using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Text;

namespace PBEdit
{
    struct Property
    {
        public string name;
        public bool isReadOnly;
        public bool hasDeafultValue;
        public string type;
        public string defaultValue;
        public string TypeHint;
    };

    class ClassSchemaXML
    {
        #region Private Fields

        private static XElement classSchemaXML;
        private static string[] componentList = new string[50];
        private static int m_componentCount = 0;

        #endregion

        #region Constructor

        public ClassSchemaXML()
        {
        }
        #endregion

        /// <summary>
        /// Get Class Schema
        /// </summary>
        /// <returns></returns>
        public static XElement GetclassSchemaXML()
        {
            return classSchemaXML;
        }

        /// <summary>
        /// Set Class Schema
        /// </summary>
        /// <returns></returns>
        public static void SetclassSchemaXML(XElement schema)
        {
            classSchemaXML = schema;
        }

        public static void SetclassSchemaXML(string schema)
        {
            schema = schema.Replace("::", ".");
            classSchemaXML = XElement.Parse(schema);
        }

        /// <summary>
        /// Get All Components Schema
        /// </summary>
        /// <returns></returns>
        public static int GetComponents(string[] componentList_)
        {
            ParseComponents();
            componentList.CopyTo(componentList_, 0);
            return m_componentCount;
        }
        /// <summary>
        /// Get All Components Schema
        /// </summary>
        /// <returns></returns>
        public static bool IsComponent(string objecttype)
        {
            if (componentList.Contains(objecttype))
                return true;

            return false;
        }

        /// <summary>
        /// Get All Components Schema
        /// </summary>
        /// <returns></returns>
        public static int ParseComponents()
        {
            //Console.Write("Schema Recieved \n" + entityXML);
            //componentList = new string[50];
            var types = from type in classSchemaXML.Descendants("type")
                        select new
                        {
                            name = type.Attribute("name").Value,
                            def = type.Descendants("factory"),
                        };

            int j = 0;
            foreach (var type in types)
            {
                string name = "Name: " + type.name;
                Console.WriteLine(name);
                Console.WriteLine(type.def.ToString());

                var components = from component in type.def.Descendants("implementsInterface")
                                 where component.Attribute("type").Value.ToString() == "com.pblabs.engine.entity.IEntityComponent"
                                 select new
                                 {
                                     Comptype = component.Attribute("type").Value,
                                 };
                int i = 0;
                foreach (var component in components)
                {
                    i++;
                }
                if (i > 0)
                {
                    componentList[j] = type.name;
                    j++;
                }
            }
            m_componentCount = j;
            return j;
        }

        /// <summary>
        /// Get All Components Schema
        /// </summary>
        /// <returns></returns>
        public static int GetClassesOfType(string type_, string[] classList_)
        {
            var types = from type in classSchemaXML.Descendants("type")
                        select new
                        {
                            name = type.Attribute("name").Value,
                            def = type.Descendants("factory"),
                        };

            int j = 0;
            foreach (var type in types)
            {
                string name = "Name: " + type.name;
                Console.WriteLine(name);
                Console.WriteLine(type.def.ToString());

                //get the actual class
                var classes = from classname in type.def
                              where classname.Attribute("type").Value == type_
                              select new
                              {
                                  classname = classname.Attribute("type").Value,
                                  metadata = classname.Descendants("metadata"),
                              };

                int i = 0;
                Boolean ignoreclass = false;
                foreach (var classnames in classes)
                {
                    i++;
                    if (classnames.metadata != null)
                    {
                        foreach (var meta in classnames.metadata)
                        {
                            var ignores = from ignore in meta.Descendants("arg")
                                          where ignore.Attribute("key").Value.ToString() == "ignore"
                                              && ignore.Attribute("value").Value.ToString() == "true"
                                          select new
                                          {
                                              key = ignore.Attribute("key").Value,
                                              value = ignore.Attribute("value").Value,
                                          };
                            foreach (var ignore in ignores)
                            {
                                ignoreclass = true;
                            }
                        }
                    }
                }

                if (i > 0 && !ignoreclass)
                {
                    classList_[j] = type.name;
                    j++;
                }

                //get the child classes
                var extendsClasses = type.def.Descendants("extendsClass");
                var metadata = type.def.Descendants("metadata");
                var extendedClasses = from extendedClass in extendsClasses
                                      where extendedClass.Attribute("type").Value.ToString().Replace("::", ".") == type_
                                      select new
                                      {
                                          name = extendedClass.Attribute("type").Value,
                                      };
                i = 0;
                ignoreclass = false;
                foreach (var extendedClass in extendedClasses)
                {
                    i++;
                    //TO DO:check  for meta data ignore here
                    foreach (var meta in metadata)
                    {
                        var ignores = from ignore in meta.Descendants("arg")
                                      where ignore.Attribute("key").Value.ToString() == "ignore"
                                          && ignore.Attribute("value").Value.ToString() == "true"
                                      select new
                                      {
                                          key = ignore.Attribute("key").Value,
                                          value = ignore.Attribute("value").Value,
                                      };
                        foreach (var ignore in ignores)
                        {
                            ignoreclass = true;
                        }
                    }
                }
                if (i > 0 && !ignoreclass)
                {
                    classList_[j] = type.name;
                    j++;
                }

            }
            return j;
        }

        public static int GetProperty(string className, ref PBEdit.Property property)
        {
            var types = from type in ClassSchemaXML.GetclassSchemaXML().Descendants("type")
                        where type.Attribute("name").Value.ToString() == className
                        select new
                        {
                            name = type.Attribute("name").Value,
                            def = type.Element("factory"),
                        };
            foreach (var type in types)                             //Type 
            {
                string name = "Name: " + type.name;
                Console.WriteLine(name);
                Console.WriteLine(type.def.ToString());

                //var metadata = type.def.Descendants("metadata");  //Meta data on the class
                var metadatas = from metadata in type.def.Elements("metadata")
                                where metadata.Attribute("name").Value.ToString() == "EditorData"
                                select new
                                {
                                    def = metadata,
                                };
                foreach (var metadata in metadatas)
                {
                    var args = from arg in metadata.def.Descendants("arg")
                               select new
                               {
                                   key = arg.Attribute("key").Value,
                                   value = arg.Attribute("value").Value,
                               };

                    foreach (var arg in args)
                    {
                        if (arg.key == "editAs")
                            property.type = arg.value;
                        if (arg.key == "typeHint")
                            property.TypeHint = arg.value;
                        if (arg.key == "ignore" && arg.value == "true")
                            return -1;
                    }
                }
            }

            return 1;
        }

        /// <summary>
        /// Get all properties of a class 
        /// </summary>
        /// <returns></returns>
        public static int GetProperties(string className, PBEdit.Property[] property)
        {
            var types = from type in ClassSchemaXML.GetclassSchemaXML().Descendants("type")
                        where type.Attribute("name").Value.ToString() == className
                        select new
                        {
                            name = type.Attribute("name").Value,
                            def = type.Descendants("factory"),
                        };

            int i = 0;
            bool next = false;
            
            foreach (var type in types)                             //Type 
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
                                        def = accessor,
                                    };

                foreach (var acc in accessorsList)
                {
                    next = false;
                    //if ignore return START
                    var metas = from meta in acc.def.Descendants("metadata")
                                select new
                                {
                                    name = meta.Attribute("name").Value,
                                    def = meta,
                                };

                    foreach (var meta in metas)
                    {
                        var ignores = from ignore in meta.def.Descendants("arg")
                                      where ignore.Attribute("key").Value.ToString() == "ignore"
                                          && ignore.Attribute("value").Value.ToString() == "true"
                                      select new
                                      {
                                          key = ignore.Attribute("key").Value,
                                          value = ignore.Attribute("value").Value,
                                      };
                        int count = 0;
                        foreach (var ignore in ignores)
                        {
                            //Console.WriteLine("Ignore: " + acc.name);
                            count++;
                        }
                        if (count > 0) //if ignore continue to next property
                        {
                            next = true;
                        }

                        var defaultValues = from defaultValue in meta.def.Descendants("arg")
                                            where defaultValue.Attribute("key").Value.ToString() == "defaultValue"
                                            select new
                                            {
                                                key = defaultValue.Attribute("key").Value,
                                                value = defaultValue.Attribute("value").Value,
                                            };
                        count = 0;
                        foreach (var defaultValue in defaultValues)
                        {
                            property[i].hasDeafultValue = true;
                            property[i].defaultValue = defaultValue.value;  //default Value
                        }

                    }//if ignore return END

                    if (next == true) //if ignore continue to next property
                    {
                        continue;
                    }
                    property[i].name = acc.name; //name
                    if (acc.access == "isReadOnlyonly")
                        property[i].isReadOnly = true; //isReadOnlyonly
                    else
                        property[i].isReadOnly = false; //isReadOnlyonly
                    if (acc.type.ToString() == "Number")
                    {
                        property[i].type = "Number";
                        if (property[i].defaultValue == null)
                        {
                            property[i].defaultValue = "0";
                        }
                    }
                    else
                    if (acc.type.ToString() == "int" || acc.type.ToString() == "uint")
                    {
                        property[i].type = "int";
                        if (property[i].defaultValue == null)
                        {
                            property[i].defaultValue = "0";
                        }
                    }
                    else
                        if (acc.type.ToString() == "Boolean")
                        {
                            property[i].type = "Boolean";
                            if (property[i].defaultValue == null)
                            {
                                property[i].defaultValue = "false";
                            }
                        }
                        else
                            if (acc.type.ToString() == "String")
                            {
                                property[i].type = "String";
                                if (property[i].defaultValue == null)
                                {
                                    property[i].defaultValue = "";
                                }
                            }
                            else
                                if (acc.type.ToString() == "flash.geom.Point")
                                {
                                    property[i].type = "Point";
                                    if (property[i].defaultValue == null)
                                    {
                                        property[i].defaultValue = "0|0";
                                    }
                                }
                                else
                                    //if (acc.type.ToString() == "com.pblabs.engine.core.ObjectType")
                                    //{
                                    //    property[i].type = "ObjectType";
                                    //    if (property[i].defaultValue == null)
                                    //    {
                                    //        property[i].defaultValue = "";
                                    //    }
                                    //}
                                    //else
                                    if (acc.type.ToString() == "Array" || acc.type.ToString() == "flash.utils.Dictionary")
                                        {
                                            property[i].type = "Array";

                                            if (property[i].defaultValue == null)
                                            {
                                                property[i].defaultValue = "";
                                            }
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
                                                    if (acc.type.ToString() == "Array")
                                                        property[i].TypeHint = arg.value+"(Array)";
                                                    else
                                                        property[i].TypeHint = arg.value + "(Dictionary)";
                                                }
                                            }
                                        }
                                        else
                                            if (acc.type.ToString() == "com.pblabs.engine.entity.PropertyReference")
                                            {
                                                property[i].type = "PropertyReference";
                                                if (property[i].defaultValue == null)
                                                {
                                                    property[i].defaultValue = "PropertyReference";
                                                }
                                            }
                                            else
                                                if (IsComponent(acc.type.ToString()))
                                                {
                                                    property[i].type = "componentReference";
                                                    if (property[i].defaultValue == null)
                                                    {
                                                        property[i].defaultValue = "componentReference";
                                                    }
                                                }
                                                else
                                                {
                                                    property[i].type = "Object";
                                                    property[i].TypeHint = "";
                                                    if (property[i].defaultValue == null)
                                                    {
                                                        property[i].defaultValue = "";
                                                    }
                                                    bool typefound = false;
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
                                                            property[i].TypeHint = arg.value;
                                                            typefound = true;
                                                        }
                                                    }
                                                    if (!typefound)
                                                    {
                                                        property[i].TypeHint = acc.type.ToString();
                                                        //if (GetProperty(acc.type.ToString(), ref property[i]) == -1)
                                                         //   continue;
                                                    }
                                                }
                    i++;
                }

                var variablesList = from variable in variables
                                    select new
                                    {
                                        name = variable.Attribute("name").Value,
                                        type = variable.Attribute("type").Value,
                                        access = "isReadOnlywrite",
                                        def = variable,
                                    };

                foreach (var acc in variablesList)
                {
                    next = false;
                    //if ignore return START
                    var metas = from meta in acc.def.Descendants("metadata")
                                select new
                                {
                                    name = meta.Attribute("name").Value,
                                    def = meta,
                                };

                    foreach (var meta in metas)
                    {
                        var ignores = from ignore in meta.def.Descendants("arg")
                                      where ignore.Attribute("key").Value.ToString() == "ignore"
                                          && ignore.Attribute("value").Value.ToString() == "true"
                                      select new
                                      {
                                          key = ignore.Attribute("key").Value,
                                          value = ignore.Attribute("value").Value,
                                      };
                        int count = 0;
                        foreach (var ignore in ignores)
                        {
                            count++;
                        }
                        if (count > 0) //if ignore continue to next property
                            next = true;

                        var defaultValues = from defaultValue in meta.def.Descendants("arg")
                                            where defaultValue.Attribute("key").Value.ToString() == "defaultValue"
                                            select new
                                            {
                                                key = defaultValue.Attribute("key").Value,
                                                value = defaultValue.Attribute("value").Value,
                                            };
                        count = 0;
                        foreach (var defaultValue in defaultValues)
                        {
                            property[i].defaultValue = defaultValue.value;  //default Value
                        }

                    }//if ignore return END

                    if (next == true) //if ignore continue to next property
                    {
                        continue;
                    }

                    property[i].name = acc.name; //name
                    if (acc.access == "isReadOnlyonly")
                        property[i].isReadOnly = true; //isReadOnlyonly
                    else
                        property[i].isReadOnly = false;

                    if (acc.type.ToString() == "Number")
                    {
                        property[i].type = "Number";
                        if (property[i].defaultValue == null)
                        {
                            property[i].defaultValue = "0";
                        }
                    }
                    else
                    if (acc.type.ToString() == "int" || acc.type.ToString() == "uint")
                    {
                        property[i].type = "int";
                        if (property[i].defaultValue == null)
                        {
                            property[i].defaultValue = "0";
                        }
                    }
                    else
                        if (acc.type.ToString() == "Boolean")
                        {
                            property[i].type = "Boolean";
                            if (property[i].defaultValue == null)
                            {
                                property[i].defaultValue = "false";
                            }
                        }
                        else
                            if (acc.type.ToString() == "String")
                            {
                                property[i].type = "String";
                                if (property[i].defaultValue == null)
                                {
                                    property[i].defaultValue = " ";
                                }
                            }
                            else
                                if (acc.type.ToString() == "flash.geom.Point")
                                {
                                    property[i].type = "Point";
                                    if (property[i].defaultValue == null)
                                    {
                                        property[i].defaultValue = "0|0";
                                    }
                                }
                                else
                                    //if (acc.type.ToString() == "com.pblabs.engine.core.ObjectType")
                                    //{
                                    //    property[i].type = "ObjectType";
                                    //    if (property[i].defaultValue == null)
                                    //    {
                                    //        property[i].defaultValue = "";
                                    //    }
                                    //}
                                    //else
                                    if (acc.type.ToString() == "Array" || acc.type.ToString() == "flash.utils.Dictionary")
                                        {
                                            property[i].type = "Array";
                                            if (property[i].defaultValue == null)
                                            {
                                                property[i].defaultValue = "";
                                            }
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
                                                    if (acc.type.ToString() == "Array")
                                                        property[i].TypeHint = arg.value + "(Array)";
                                                    else
                                                        property[i].TypeHint = arg.value + "(Dictionary)";
                                                }
                                            }
                                        }
                                        else
                                            if (acc.type.ToString() == "com.pblabs.engine.entity.PropertyReference")
                                            {
                                                property[i].type = "PropertyReference";
                                                if (property[i].defaultValue == null)
                                                {
                                                    property[i].defaultValue = "PropertyReference";
                                                }
                                            }
                                            else
                                                if (IsComponent(acc.type.ToString()))
                                                {
                                                    property[i].type = "componentReference";
                                                    if (property[i].defaultValue == null)
                                                    {
                                                        property[i].defaultValue = "componentReference";
                                                    }
                                                }
                                                else
                                                {
                                                    property[i].type = "Object";
                                                    property[i].TypeHint = "";
                                                    if (property[i].defaultValue == null)
                                                    {
                                                        property[i].defaultValue = "";
                                                    }
                                                    bool typefound = false;
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
                                                            property[i].TypeHint = arg.value;
                                                            typefound = true;
                                                        }
                                                    }
                                                    if (!typefound)
                                                    {
                                                        property[i].TypeHint = acc.type.ToString();
                                                        //if (GetProperty(acc.type.ToString(),ref property[i]) == -1)
                                                        //    continue;
                                                    }
                                                }
                    i++;
                }
            } //foreach (var type in types)     END
            return i;
        }


    }
}
