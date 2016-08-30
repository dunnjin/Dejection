using Osc.Dejection.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

namespace Osc.Dejection.Framework
{
    public class DataTemplateService : IDataTemplateService
    {
        #region Fields

        private IList<DataTemplate> dataTemplateCollection = new List<DataTemplate>();

        #endregion

        #region Methods

        /// <summary>
        /// Applies the found ViewModel / View datatemplates to the windows resources
        /// </summary>
        /// <param name="content"></param>
        public void InjectDataTemplates(Window content)
        {
            foreach(DataTemplate dataTemplate in dataTemplateCollection)
            {
                content.Resources.Add(dataTemplate.DataTemplateKey, dataTemplate);
            }
        }
        
        /// <summary>
        /// Based off the given pattern looks at loaded dlls in the program for the association of ViewModel / Views based of naming and creates the datatemplate to be stored in memory
        /// </summary>
        /// <param name="predicate">The pattern used to determine what dlls that are loaded in memory to look for ViewModel / View convention</param>
        public void GenerateDataTemplates(Func<Assembly, bool> predicate)
        {
            foreach(Assembly assembly in AppDomain.CurrentDomain.GetAssemblies()
                .Where(predicate))
            {
                List<Type> viewTypes = new List<Type>(assembly.GetTypes()
                    .Where(obj => typeof(ContentControl).IsAssignableFrom(obj))
                    .Where(obj => obj.Name.EndsWith("View")));

                List<Type> viewModelTypes = new List<Type>(assembly.GetTypes()
                    .Where(obj => typeof(ViewModelBase).IsAssignableFrom(obj))
                    .Where(obj => obj.Name.EndsWith("ViewModel")));

                foreach(Type view in viewTypes)
                {
                    string viewName = view.Name;

                    Type viewModel = viewModelTypes
                        .FirstOrDefault(obj => obj.Name == $"{viewName}Model");

                    if (viewModel.IsNull())
                        continue;

                    DataTemplate dataTemplate = CreateTemplate(viewModel, view);

                    dataTemplateCollection.Add(dataTemplate);
                }                            
            }
        }

        //public DataTemplate GetTemplate<TSource>()
        // where TSource : ViewModelBase
        //{
        //    return CreateTemplate(typeof(TSource), GetViewFromViewModel<TSource>());
        //}

        //private Type GetViewFromViewModel<TSource>()
        //    where TSource : ViewModelBase
        //{
        //    return viewTypes.FirstOrDefault(obj => obj.Name == typeof(TSource).Name.Replace("Model", string.Empty));
        //}
        
        private DataTemplate CreateTemplate(Type viewModelType, Type viewType)
        {
            const string xamlTemplate = "<DataTemplate DataType=\"{{x:Type vm:{0}}}\"><v:{1} /></DataTemplate>";
            var xaml = String.Format(xamlTemplate, viewModelType.Name, viewType.Name, viewModelType.Namespace, viewType.Namespace);

            var context = new ParserContext();

            context.XamlTypeMapper = new XamlTypeMapper(new string[0]);
            context.XamlTypeMapper.AddMappingProcessingInstruction("vm", viewModelType.Namespace, viewModelType.Assembly.FullName);
            context.XamlTypeMapper.AddMappingProcessingInstruction("v", viewType.Namespace, viewType.Assembly.FullName);

            context.XmlnsDictionary.Add("", "http://schemas.microsoft.com/winfx/2006/xaml/presentation");
            context.XmlnsDictionary.Add("x", "http://schemas.microsoft.com/winfx/2006/xaml");
            context.XmlnsDictionary.Add("vm", "vm");
            context.XmlnsDictionary.Add("v", "v");

            var the = XamlReader.Parse(xaml, context).ToString();

            var template = (DataTemplate)XamlReader.Parse(xaml, context);
            return template;
        }

     

        #endregion
    }
}
