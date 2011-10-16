using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Drawing;

namespace PBEdit
{
    class Point2D
    {
        private float X;
        private float Y;

        public Point2D(float x_,float y_)
        {
            X = x_; Y = y_;
        }

        public float x
        {
            get
            {
                return X;
            }
            set
            {
                X = value;
            }
        }

        public float y
        {
            get
            {
                return Y;
            }
            set
            {
                Y = value;
            }
        }
    };

    class PBEditUtils
    {
        /// <summary>
        /// Add Components to PropertyGridEx
        /// </summary>
        /// <returns></returns>
        public static void AddComponetToPropertyGridEx(string component_, string category_, PropertyGridEx.PropertyGridEx p)
        {
            //Console.Write("Schema Recieved \n" + entityXML);
            p.Item.Clear();
            var types = from type in ClassSchemaXML.GetclassSchemaXML().Descendants("type")
                        where type.Attribute("name").Value.ToString().Replace("::", ".") == component_
                        select new
                        {
                            name = type.Attribute("name").Value,
                            def = type.Descendants("factory"),
                        };

            foreach (var type in types)
            {
                string name = "Name: " + type.name;
                Console.WriteLine(name);
                Console.WriteLine(type.def.ToString());

                var accessors = type.def.Descendants("accessor");
                var variables = type.def.Descendants("variable");

                var accessorsList = from accessor in accessors
                                    select new
                                    {
                                        name = accessor.Attribute("name").Value,
                                        access = accessor.Attribute("access").Value,
                                        type = accessor.Attribute("type").Value,
                                        def = accessor,//.Descendants("metadata"),
                                    };

                //XElement comp = new XElement("component", new XAttribute("type", component_), new XAttribute("name", "Spatial")
                //                            );
                foreach (var acc in accessorsList)
                {

                    if (acc.type.ToString() == "Number" || acc.type.ToString() == "int" || acc.type.ToString() == "uint")
                    {
                        p.Item.Add(acc.name, 0, false, category_, "", true);
                        //comp.Add(new XElement(acc.name, 0));
                    }

                    if (acc.type.ToString() == "Boolean")
                    {
                        p.Item.Add(acc.name, true, false, category_, "", true);
                        //comp.Add(new XElement(acc.name, true));
                    }

                    if (acc.type.ToString() == "flash.geom::Point")
                    {
                        p.Item.Add(acc.name, new Point2D(10, 10), false, category_, "", true);
                        //comp.Add(new XElement(acc.name, new XElement("x", 0),
                                //                        new XElement("y", 0)
                                //             )
                                //);
                    }

                    if (acc.type.ToString() == "com.pblabs.engine.core::ObjectType")
                    {
                        p.Item.Add(acc.name, "", false, category_, "", true);
                        p.Item[p.Item.Count - 1].Choices = new PropertyGridEx.CustomChoices(new string[] { "Template1", "Template2" }, true); //get all object types
                        //comp.Add(new XElement(acc.name, true));
                    }

                    if (acc.type.ToString() == "Array")
                    {
                        var metas = from meta in acc.def.Descendants("metadata")
                                    select new
                                    {
                                        name = meta.Attribute("name").Value,
                                        def = meta,
                                    };

                        foreach (var meta in metas)
                        {
                            var args = from arg in meta.def.Descendants("arg")
                                       where arg.Attribute("key").Value.ToString() == "type"
                                       select new
                                       {
                                           key = arg.Attribute("key").Value,
                                           value = arg.Attribute("value").Value,
                                       };
                            foreach (var arg in args)
                            {
                                p.Item.Add(acc.name, "", false, category_, "", true);
                                p.Item[p.Item.Count - 1].Value = arg.value;
                                p.Item[p.Item.Count - 1].CustomEditor = new ArrayEditorUI();// new MyEditor(arg.value);
                            }
                        }

                    }
                }

                var variablesList = from variable in variables
                                    select new
                                    {
                                        name = variable.Attribute("name").Value,
                                        type = variable.Attribute("type").Value,
                                        def = variable.Descendants("metadata"),
                                    };

                foreach (var acc in variablesList)
                {
                    if (acc.type.ToString() == "Number" || acc.type.ToString() == "int")
                        p.Item.Add(acc.name, 0, false, "Spatial", "", true);

                    if (acc.type.ToString() == "Boolean")
                        p.Item.Add(acc.name, true, false, "Spatial", "", true);

                    if (acc.type.ToString() == "flash.geom::Point")
                        p.Item.Add(acc.name, new Point(10, 10), false, "Spatial", "", true);
                }
                p.Refresh();
            }
        }
    }
}
