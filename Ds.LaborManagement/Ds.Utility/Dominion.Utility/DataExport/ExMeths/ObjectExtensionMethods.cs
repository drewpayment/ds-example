//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;

//namespace Dominion.Utility.DataExport.ExMeths
//{
//    public static class ObjectExtensionMethods
//    {
//        /// <summary>
//        /// Provide an alternative value is a object is null.
//        /// </summary>
//        /// <typeparam name="T"></typeparam>
//        /// <param name="obj"></param>
//        /// <param name="altResult"></param>
//        /// <returns></returns>
//        public static T IsNull<T>(this T obj, Func<T> altResult)
//            where T : class
//        {
//            if(obj != null)
//            {
//                var v = obj;
//                return v;
//            }
//            else
//            {
//                var v = altResult();
//                return v;
//            }       
//        }


//        public static string ObjectToHtml<T>(this IEnumerable<T> list, string title)
//        {
//            StringBuilder myBuilder = new StringBuilder();

//            //Open tags and write the top portion.
//            myBuilder.AppendLine("<html xmlns='http://www.w3.org/1999/xhtml'>");
//            myBuilder.AppendLine("<head>");
//            myBuilder.AppendLine("<title>");
//            myBuilder.AppendLine(title);
//            myBuilder.AppendLine("");
//            myBuilder.AppendLine("</title>");
//            myBuilder.AppendLine("</head>");
//            myBuilder.AppendLine("<body>");
//            myBuilder.AppendLine("<table border='1px' cellpadding='5' cellspacing='0' ");
//            myBuilder.AppendLine("style='border: solid 1px Silver; font-size: x-small;'>");

//            //Add the headings row.
//            myBuilder.AppendLine("<tr align='left' valign='top'>");

//            //ADD THE HEADER
//            var propertyArray = typeof(T).GetProperties();
//            foreach (var prop in propertyArray)
//            {
//                string info = string.Format("{0} - {1}", prop.Name, prop.PropertyType.Name);
//                myBuilder.AppendLine("<td align='left' valign='top'>");
//                myBuilder.AppendLine(info);
//                myBuilder.AppendLine("</td>");
//            }

//            //ADD THE VALUES
//            for (int i = 0; i < list.Count(); i++)
//            {
//                myBuilder.AppendLine("<tr align='left' valign='top'>");

//                foreach (var prop in propertyArray)
//                {
//                    object value = prop.GetValue(list.ElementAt(i), null);
//                    //result.AppendFormat("<td>{0}</td>", value ?? String.Empty);

//                    value = (value == null) ? DBNull.Value : value; //in case value is null

//                    myBuilder.AppendLine("<td align='left' valign='top'>");
//                    myBuilder.AppendLine(value.ToString());
//                    myBuilder.AppendLine("</td>");

//                }
//                myBuilder.AppendLine("</tr>");
//                //result.AppendLine("</tr>");
//            }

//            //Close tags.
//            myBuilder.AppendLine("</table>");
//            myBuilder.AppendLine("</body>");
//            myBuilder.AppendLine("</html>");

//            //Get the string for return.
//            string myHtmlFile = myBuilder.ToString();

//            return myHtmlFile;
//        }

//    }
//}
