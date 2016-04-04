#region *   License     *
/*
    SimpleHelpers - ObjectDiffPatch   

    Copyright © 2014 Khalid Salomão

    Permission is hereby granted, free of charge, to any person
    obtaining a copy of this software and associated documentation
    files (the “Software”), to deal in the Software without
    restriction, including without limitation the rights to use,
    copy, modify, merge, publish, distribute, sublicense, and/or sell
    copies of the Software, and to permit persons to whom the
    Software is furnished to do so, subject to the following
    conditions:

    The above copyright notice and this permission notice shall be
    included in all copies or substantial portions of the Software.

    THE SOFTWARE IS PROVIDED “AS IS”, WITHOUT WARRANTY OF ANY KIND,
    EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
    OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
    NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
    HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
    WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
    FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
    OTHER DEALINGS IN THE SOFTWARE. 

    License: http://www.opensource.org/licenses/mit-license.php
    Website: https://github.com/khalidsalomao/SimpleHelpers.Net
 */

#endregion

// Note mkoertgen: copied tests from original repo.

using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;

namespace FluentAssertions.Json
{
    [TestClass]
    public class ObjectDiffPatchTest
    {
        private TestClass GetSimpleTestObject()
        {
            return new TestClass()
            {
                StringProperty = "this is a string",
                IntProperty = 1234,
                DoubleProperty = 56.789
            };
        }

        private void PopulateStringListOnTestClass(TestClass testObject)
        {
            testObject.ListOfStringProperty = new List<string>()
            {
                "this", "is", "a", "list", "of", "strings"
            };
        }

        private void PopulateObjectListOnTestClass(TestClass testObject)
        {
            testObject.ListOfObjectProperty = new List<TestClass>()
            {
                new TestClass()
                {
                    StringProperty = "this is the first object",
                    IntProperty = 1,
                    DoubleProperty = 1.01
                },
                new TestClass()
                {
                    StringProperty = "this is the second object",
                    IntProperty = 2,
                    DoubleProperty = 2.02
                },
                new TestClass()
                {
                    StringProperty = "this is the third object",
                    IntProperty = 3,
                    DoubleProperty = 3.03
                }
            };
        }

        [TestMethod]
        public void AbleToDiffAndPatchSimpleObject()
        {
            var testObj = GetSimpleTestObject();

            var updatedTestObj = GetSimpleTestObject();
            updatedTestObj.StringProperty = "this is an updated string";
            updatedTestObj.IntProperty = 5678;
            updatedTestObj.DoubleProperty = 123.456;

            var diff = ObjectDiffPatch.GenerateDiff(testObj, updatedTestObj);

            var revertPatch = JsonConvert.SerializeObject(diff.OldValues);

            var revertedObj = ObjectDiffPatch.PatchObject(updatedTestObj, revertPatch);

            Assert.AreEqual(testObj.StringProperty, revertedObj.StringProperty);
            Assert.AreEqual(testObj.IntProperty, revertedObj.IntProperty);
            Assert.AreEqual(testObj.DoubleProperty, revertedObj.DoubleProperty);
        }

        [TestMethod]
        public void AbleToDeleteStringListItemThenRevertViaPatch()
        {
            var testObj = GetSimpleTestObject();
            PopulateStringListOnTestClass(testObj);

            var updatedTestObj = GetSimpleTestObject();
            PopulateStringListOnTestClass(updatedTestObj);

            updatedTestObj.ListOfStringProperty.Remove("list");

            CollectionAssert.AreNotEqual(testObj.ListOfStringProperty, updatedTestObj.ListOfStringProperty);


            var diff = ObjectDiffPatch.GenerateDiff(testObj, updatedTestObj);

            var revertPatch = JsonConvert.SerializeObject(diff.OldValues);

            var revertedObj = ObjectDiffPatch.PatchObject(updatedTestObj, revertPatch);

            CollectionAssert.AreEqual(testObj.ListOfStringProperty, revertedObj.ListOfStringProperty);
        }

        [TestMethod]
        public void AbleToDeleteObjectListItemThenRevertViaPatch()
        {
            var testObj = GetSimpleTestObject();
            PopulateObjectListOnTestClass(testObj);

            var updatedTestObj = GetSimpleTestObject();
            PopulateObjectListOnTestClass(updatedTestObj);

            updatedTestObj.ListOfObjectProperty.RemoveAt(1);

            Assert.AreNotEqual(testObj.ListOfObjectProperty.Count, updatedTestObj.ListOfObjectProperty.Count);

            var diff = ObjectDiffPatch.GenerateDiff(testObj, updatedTestObj);

            var revertPatch = JsonConvert.SerializeObject(diff.OldValues);

            var revertedObj = ObjectDiffPatch.PatchObject(updatedTestObj, revertPatch);

            Assert.AreEqual(testObj.ListOfObjectProperty.Count, revertedObj.ListOfObjectProperty.Count);
        }

        [TestMethod]
        public void AbleToEditObjectInListThenRevertViaPatch()
        {
            var testObj = GetSimpleTestObject();
            PopulateObjectListOnTestClass(testObj);

            var updatedTestObj = GetSimpleTestObject();
            PopulateObjectListOnTestClass(updatedTestObj);

            updatedTestObj.ListOfObjectProperty[2].IntProperty = 30;
            updatedTestObj.ListOfObjectProperty[2].StringProperty = "this is an update to the last object in the list";
            updatedTestObj.ListOfObjectProperty[2].DoubleProperty = 33.333;

            var diff = ObjectDiffPatch.GenerateDiff(testObj, updatedTestObj);

            var revertPatch = JsonConvert.SerializeObject(diff.OldValues);

            var revertedObj = ObjectDiffPatch.PatchObject(updatedTestObj, revertPatch);

            Assert.AreEqual(testObj.ListOfObjectProperty[2].IntProperty, revertedObj.ListOfObjectProperty[2].IntProperty);
            Assert.AreEqual(testObj.ListOfObjectProperty[2].StringProperty, revertedObj.ListOfObjectProperty[2].StringProperty);
            Assert.AreEqual(testObj.ListOfObjectProperty[2].DoubleProperty, revertedObj.ListOfObjectProperty[2].DoubleProperty);
        }

        [TestMethod]
        public void AbleToAddObjectListItemThenApplyViaPatch()
        {
            var testObj = GetSimpleTestObject();
            PopulateObjectListOnTestClass(testObj);

            var updatedTestObj = GetSimpleTestObject();
            PopulateObjectListOnTestClass(updatedTestObj);

            updatedTestObj.ListOfObjectProperty.Add(new TestClass { StringProperty = "added" });

            var diff = ObjectDiffPatch.GenerateDiff(testObj, updatedTestObj);

            var updatePatch = JsonConvert.SerializeObject(diff.NewValues);

            var objToUpdate = GetSimpleTestObject();
            PopulateObjectListOnTestClass(objToUpdate);

            var updatedObj = ObjectDiffPatch.PatchObject(objToUpdate, updatePatch);

            Assert.AreEqual(updatedTestObj.ListOfObjectProperty.Count, updatedObj.ListOfObjectProperty.Count);

            var addedListItem = updatedObj.ListOfObjectProperty.SingleOrDefault(obj => obj != null && obj.StringProperty == "added");

            Assert.IsNotNull(addedListItem);

        }
    }

    internal class TestClass
    {
        public string StringProperty { get; set; }
        public int IntProperty { get; set; }
        public double DoubleProperty { get; set; }
        public List<TestClass> ListOfObjectProperty { get; set; }
        public List<string> ListOfStringProperty { get; set; }
        public List<int> ListOfIntProperty { get; set; }
        public List<double> ListOfDoubleProperty { get; set; }
    }
}