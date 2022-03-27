using My_IRAS_Project.classes;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

namespace My_IRAS_Project
{
    static  class Program
    {


        static void Main(string[] args)
        {
            List<Summary> list = new List<Summary>();
            var currentValue = "";
            var revisedValue = "";
            var label = string.Empty;


           // Extended extended = new Extended();

           // test something


           //   List<dynamic> propertyList = new List<dynamic>();

            List<Property> current = new List<Property>()
            { 
                new Property("Address B", new List<string>(){ "10-156", "3-14"}, new List<string>(){ "DogA", "DogB"}),
                new Property("Address A", new List<string>(){ "10-256", "3-14"}, new List<string>(){ "DogC", "DogD"})
            };

            List<Property> revised = new List<Property>()
            {
                new Property("Address A", new List<string>(){ "10-256", "6-14", "7-15"}, new List<string>(){ "DogA", "DogB"}),
                new Property("Address C", new List<string>(){ "10-256", "3-14"}, new List<string>(){ "CatA", "CatB"}),
            };

            //property config
            Type type = typeof(Property);
            var identifier = type.GetProperty("Address").Name;
            var firstObjectToCompare = type.GetProperty("Units");
            var secondObjectToCompare = type.GetProperty("Pets");

            var objectsToCompare = new PropertyInfo[] { firstObjectToCompare, secondObjectToCompare };

            var res = DeepCompare(current, revised, identifier, objectsToCompare);

            foreach (var item in revised)
            {
                var currentMatchedPropertyItem = current.Where(x => x.Address == item.Address).FirstOrDefault();
                //var currentMatchedPropertyItemIndex = current.IndexOf(currentMatchedPropertyItem);

                if(currentMatchedPropertyItem != null)
                {
                    label = "Address";
                    currentValue = currentMatchedPropertyItem.Address;
                    revisedValue = currentValue;

                    //list.Add(new Summary(label, currentValue, revisedValue));
                    var testObjectSetValues = new Summary(label, currentValue, revisedValue);

                    testObjectSetValues.SetValue(typeof(Property), false);

                    foreach (var unitLevel in item.Units)
                    {
                        var currentMatchedUnitLevelItem = currentMatchedPropertyItem
                            .Units.
                            Where(x => x == unitLevel).FirstOrDefault();

                        if (currentMatchedUnitLevelItem != null)
                        {
                            label = "unit-level";
                            currentValue = currentMatchedUnitLevelItem;
                            revisedValue = currentValue;
                            list.Add(new Summary(label, currentValue, revisedValue));

                            currentMatchedPropertyItem.Units.Remove(currentMatchedUnitLevelItem);
                        }

                        else
                        {
                            label = "unit-level";
                            currentValue = "NotApplicable";
                            revisedValue = unitLevel;
                            list.Add(new Summary(label, currentValue, revisedValue));
                        }
                    }

                    //add removed units current
                    foreach (var removedUnits in currentMatchedPropertyItem.Units)
                    {
                        label = "unit-level";
                        currentValue = removedUnits;
                        revisedValue = "Removed";
                        list.Add(new Summary(label, currentValue, revisedValue));
                    }

                    current.Remove(currentMatchedPropertyItem);
  
                   // extended.Summary = list;
                }

                else
                {
                    label = "Address";
                    revisedValue = item.Address;
                    currentValue = "NotApplicable";
                    list.Add(new Summary(label, currentValue, revisedValue));

                    foreach (var newUnitsAdded in item.Units)
                    {
                        label = "unit-level";
                        revisedValue = newUnitsAdded;
                        currentValue = "NotApplicable";
                        list.Add(new Summary(label, currentValue, revisedValue));
                    }
                }

            }

            // Add removed properties

            foreach(var removedProperties in current)
            {

                label = "Address";
                revisedValue = "Not Applicable";
                currentValue = removedProperties.Address;
                list.Add(new Summary(label, currentValue, revisedValue));

                foreach (var removedLevelUnits in removedProperties.Units)
                {
                    label = "unit-level";
                    revisedValue = "Not Applicable";
                    currentValue = removedLevelUnits;
                    list.Add(new Summary(label, currentValue, revisedValue));
                }
            }

        }

        public static List<List<Summary>> DeepCompare<T>(List<T> currentListOfObjects, List<T> revisedListOfObjects, string identifier, PropertyInfo[] listOfObjectsToCompare)
        {
            var label = string.Empty;
            var currentValue = string.Empty;
            var revisedValue = string.Empty;
            var finalList = new List<List<Summary>>();

            foreach (var item in revisedListOfObjects)
            {
                //PropertyInfo addressProperty = typeof(T).GetProperty(identifier);
                var list = new List<Summary>();

                var itemValue = item.GetType().GetProperty(identifier).GetValue(item, null);

                var currentMatchedItem = currentListOfObjects.Where(
                    x => x.GetType().GetProperty(identifier)
                    .GetValue(x, null) == itemValue)
                    .FirstOrDefault();

                var itemDisplayName = (DisplayAttribute)item.GetType().GetProperty(identifier).GetCustomAttribute(typeof(DisplayAttribute));

                if (currentMatchedItem != null)
                {

                    var sumObj = new Summary(itemDisplayName.Name, itemValue.ToString(), itemValue.ToString());
                    sumObj.SetValue(typeof(T), true);
                    list.Add(sumObj);   

                    foreach (PropertyInfo objectToCompare in listOfObjectsToCompare)
                    {
                        //retrieve property displayname set for the object
                        var objectDisplayName = (DisplayAttribute)objectToCompare.GetCustomAttribute(typeof(DisplayAttribute));

                        //first Object to Compare
                        var revisedObjListToCompare = item.GetType().GetProperty(objectToCompare.Name).GetValue(item, null);

                        if (revisedObjListToCompare is List<string>)
                        {
                            foreach (var revisedObjToCompare in (List<string>)revisedObjListToCompare)
                            {
                                var currentMatchedFirstObjList = (List<string>)currentMatchedItem
                                    .GetType()
                                    .GetProperty(objectToCompare.Name).GetValue(currentMatchedItem, null);

                                var currentMatchedFirstObj = currentMatchedFirstObjList.Where(x => x == revisedObjToCompare).FirstOrDefault();

                                if (currentMatchedFirstObj != null)
                                {
                                    //setvalues
                                    var sumObj2 = new Summary(objectDisplayName.Name, currentMatchedFirstObj, currentMatchedFirstObj);
                                    sumObj2.SetValue(typeof(T), false);
                                    list.Add(sumObj2);
                                    

                                    currentMatchedFirstObjList.Remove(currentMatchedFirstObj);
                                }

                                //adding of new objects to the summary comparison
                                else
                                {
                                    //setvalues
                                    var sumObj2 = new Summary(objectDisplayName.Name, null, revisedObjToCompare);
                                    sumObj2.SetValue(typeof(T), false);
                                    list.Add(sumObj2);

                                }

                            }

                            #region Adding of current removed objects to summary comparison

                            //Remaining currentObjectsList to compare after removing matched items from current list of objects

                            var currentObjectsList = (List<string>)currentMatchedItem
                                .GetType()
                                .GetProperty(objectToCompare.Name)
                                .GetValue(currentMatchedItem, null);

                            foreach (var currentRemovedObject in currentObjectsList)
                            {
                                //setvalues
                                var sumObj2 = new Summary(objectDisplayName.Name, currentRemovedObject, null);
                                sumObj2.SetValue(typeof(T), false);
                                list.Add(sumObj2);

                            }

                            #endregion

                            currentListOfObjects.Remove(currentMatchedItem);
                        }
                    }
                }

                //adding of new revised items  

                else
                {
                    //setvalues
                    var sumObj2 = new Summary(itemDisplayName.Name, itemValue.ToString(), itemValue.ToString());
                    sumObj2.SetValue(typeof(T), true);
                    list.Add(sumObj2);

                    foreach (PropertyInfo objectToCompare in objectsToCompare)
                    {
                        var objectDisplayName = (DisplayAttribute)objectToCompare.GetCustomAttribute(typeof(DisplayAttribute));

                        var revisedNewObjectsList = (List<string>)item
                                                .GetType()
                                                .GetProperty(objectToCompare.Name)
                                                .GetValue(item, null);

                        foreach (var newUnitsAdded in revisedNewObjectsList)
                        {
                            //setvalues
                            var sumObj3 = new Summary(objectDisplayName.Name, null, newUnitsAdded);
                            sumObj3.SetValue(typeof(T), false);
                            list.Add(sumObj3);

                        }
                    }
                }
                finalList.Add(list);
            }

            //Add reamining removed items from current 

            foreach(var item in currentListOfObjects)
            {
                var list = new List<Summary>();

                var itemValue = item.GetType().GetProperty(identifier).GetValue(item, null);

                var itemDisplayName = (DisplayAttribute)item.GetType().GetProperty(identifier).GetCustomAttribute(typeof(DisplayAttribute));

                //setvalues
                var sumObj2 = new Summary(itemDisplayName.Name, itemValue.ToString(), itemValue.ToString());
                sumObj2.SetValue(typeof(T), true);
                list.Add(sumObj2);

                //add all the objects

                foreach (PropertyInfo objectToCompare in objectsToCompare)
                {
                    var objectDisplayName = (DisplayAttribute)objectToCompare.GetCustomAttribute(typeof(DisplayAttribute));

                    var currentRemovedObjectsList = (List<string>)item
                                            .GetType()
                                            .GetProperty(objectToCompare.Name)
                                            .GetValue(item, null);

                    foreach (var removedObject in currentRemovedObjectsList)
                    {
                        //setvalues
                        var sumObj3 = new Summary(objectDisplayName.Name, removedObject, null);
                        sumObj3.SetValue(typeof(T), false);
                        list.Add(sumObj3);
                    }
                }

                finalList.Add(list);

            }

            return finalList;
        }

        public static bool DeepEquals(this object obj, object another)
        {

            if (ReferenceEquals(obj, another)) return true;
            if ((obj == null) || (another == null)) return false;
            // Comparing class of 2 objects, if different, then fail
            if (obj.GetType() != another.GetType()) return false;

            var result = true;
            // Get all properties of obj
            // then compare the value of each property
            foreach (var property in obj.GetType().GetProperties())
            {
                var objValue = property.GetValue(obj);
                var anotherValue = property.GetValue(another);
                if (!objValue.Equals(anotherValue)) result = false;
            }

            return result;
        }

        //public static bool DeepEquals<T>(this IEnumerable<T> obj, IEnumerable<T> another)
        //{
        //    if (ReferenceEquals(obj, another)) return true;
        //    if ((obj == null) || (another == null)) return false;

        //    bool result = true;
        //    // Browse each element in 2 given list
        //    using (IEnumerator<T> enumerator1 = obj.GetEnumerator())
        //    using (IEnumerator<T> enumerator2 = another.GetEnumerator())
        //    {
        //        while (true)
        //        {
        //            bool hasNext1 = enumerator1.MoveNext();
        //            bool hasNext2 = enumerator2.MoveNext();

        //            // If there is 1 list, or 2 different elements, exit the loop
        //            if (hasNext1 != hasNext2)
        //            {
        //                result = false;
        //                break;
        //            }

        //            // Stop the loop when 2 lists are all
        //            if (!hasNext1) break;
        //        }
        //    }

        //    return result;
        //}



        public class Person
        {
            public string age { get; set; }
            public string name { get; set; }
            public List<Car> carList { get; set; }

            public partial class Car
            {
                public string id { get; set; }
                public string title { get; set; }
            }
            
        }

        public class compareClass : IComparer<Person>
        {
            public int Compare(Person x, Person y)
            {
                if (x == null || y == null)
                {
                    return 0;
                }

                // CompareTo() method
                return x.age.CompareTo(y.age);

            }
        }

    }
}
