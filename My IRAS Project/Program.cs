using Mapster;
using My_IRAS_Project.classes;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ObjectsComparer;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using static My_IRAS_Project.classes.Property;

namespace My_IRAS_Project
{
    static class Program
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

            List<Property> revised2 = new List<Property>()
            {
                new Property("Address B", new List<string>(){ "10-156", "3-14"}, new List<string>(){ "DogA", "DogB"}),
                new Property("Address A", new List<string>(){ "10-256", "3-14"}, new List<string>(){ "DogC", "DogD"})
            };

            List<Property> revised = new List<Property>()
            {
                new Property("Address A", new List<string>(){ "10-256", "6-14", "7-15"}, new List<string>(){ "DogA", "DogB"}),
                new Property("Address C", new List<string>(){ "10-256", "3-14"}, new List<string>(){ "CatA", "CatB"}),
                new Property("Address A", new List<string>(){ "10-256", "3-14"}, new List<string>(){ "DogC", "DogD"})
            };

            ObjectComparer pc = new ObjectComparer();

            IEnumerable<Property> except = revised2.Except(current, pc);

            bool isEqual = revised2.SequenceEqual(current, new ObjectComparer());


            var comparer = new Comparer();

            IEnumerable<Difference> differences;
            var isEqualOrNot = comparer.Compare((List<Property>)current, (List<Property>)revised2, out differences);

            //property config
            Type type = typeof(Property);
            var identifier = type.GetProperty("Address").Name;
            var firstObjectToCompare = type.GetProperty("Units");
            var secondObjectToCompare = type.GetProperty("Pets");

            var objectsToCompare = new PropertyInfo[] { firstObjectToCompare, secondObjectToCompare };

            var res = DeepCompare(current, revised, identifier, objectsToCompare);

            Moms momsA = new Moms();
            momsA.Name = "Reeta";
            Moms momsB = new Moms();
            momsB.Name = "Geetha";

            List<Moms> mom = new List<Moms>() { momsA, momsB };
  

            Property prop = new Property()
            {
                Address = "AddressB",
                Units = new List<string> { "12", "21" },
                Pets = new List<string> { "DogA", "DogB" },
                number = 1.23m,
                Momas = mom,
                date = DateTimeOffset.Parse("2022-01-04"),
                IsMyEnum = false
            };


            var Adapt = TypeAdapterConfig<Property, PropertyCardDto>.NewConfig().Map(dest => dest.Address, src => string.Format("{0} some value", src.Address))
                .Map(dest => dest.number, src => src.number)
                .Map(dest => dest.myEnum, src => src.myEnum)
                .Map(dest => dest.date, src => src.date)
                .Map(dest => dest.Moms, src => src.Momas.Select(x=>x.Name).ToList())
                .Map(dest => dest.IsMyEnum, src => (src.IsMyEnum) ? "this is true":"not true");


            PropertyCardDto destination = prop.Adapt<Property, PropertyCardDto>();

            List<PropertyDetail> propertyFinalList = new List<PropertyDetail>();

            // var prop = (List<List<Summary>>)res.GetType().GetProperty("Home Address").GetValue(res, null);
            foreach (List<Summary> result in res)
            {
                PropertyDetail finalSummaryModel = new PropertyDetail();
                var summaryListNew = new List<Summary>();
                foreach (Summary obj in result)
                {
                    if (obj._title == "Home Address")
                    {
                        finalSummaryModel.address = obj;
                    }

                    else if (obj._title == "Level-Units")
                    {
                        var newObj = new Summary(obj._title, obj._currentValue, obj._revisedValue);

                        summaryListNew.Add(newObj);

                    }
                    finalSummaryModel.unitLevels = (summaryListNew);

                }
                propertyFinalList.Add(finalSummaryModel);
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

                    foreach (PropertyInfo objectToCompare in listOfObjectsToCompare)
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

            foreach (var item in currentListOfObjects)
            {
                var list = new List<Summary>();

                var itemValue = item.GetType().GetProperty(identifier).GetValue(item, null);

                var itemDisplayName = (DisplayAttribute)item.GetType().GetProperty(identifier).GetCustomAttribute(typeof(DisplayAttribute));

                //setvalues
                var sumObj2 = new Summary(itemDisplayName.Name, itemValue.ToString(), itemValue.ToString());
                sumObj2.SetValue(typeof(T), true);
                list.Add(sumObj2);

                //add all the objects

                foreach (PropertyInfo objectToCompare in listOfObjectsToCompare)
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

        //public class compareClass : IComparer<Person>
        //{
        //    public int Compare(Person x, Person y)
        //    {
        //        if (x == null || y == null)
        //        {
        //            return 0;
        //        }

        //        // CompareTo() method
        //        return x.age.CompareTo(y.age);

        //    }
        //}

    }
}
