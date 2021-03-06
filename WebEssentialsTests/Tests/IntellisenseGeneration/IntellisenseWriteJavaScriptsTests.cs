﻿using System.Text;
using MadsKristensen.EditorExtensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WebEssentialsTests.Tests.IntellisenseGeneration
{
    [TestClass]
    public class IntellisenseJavaScript_with_primitives
    {
        private readonly IntellisenseType _stringType = new IntellisenseType { CodeName = "String" };
        private readonly IntellisenseType _int32Type = new IntellisenseType { CodeName = "Int32" };
        private readonly IntellisenseType _int32ArrayType = new IntellisenseType { CodeName = "Int32", IsArray = true };
        private readonly IntellisenseType _simpleType = new IntellisenseType { CodeName = "Foo.Simple", ClientSideReferenceName = "server.Simple" };

        [TestMethod]
        public void JavaScript_with_on_string_property()
        {
            var result = new StringBuilder();

            var io = new IntellisenseObject(new[]
            {
                new IntellisenseProperty(_stringType, "AString")
            })
            {
                FullName = "Foo.Primitives",
                Name = "Primitives",
                Namespace = "server"
            };
            IntellisenseWriter.WriteJavaScript(new[] { io }, result);

            result.ShouldBeCode(@"
var server = server || {};
/// <summary>The Primitives class as defined in Foo.Primitives</summary>
server.Primitives = function() {
/// <field name=""AString"" type=""String"">The AString property as defined in Foo.Primitives</field>
this.AString = '';
};");
        }
        [TestMethod]
        public void JavaScript_with_a_string_an_int_and_and_int_arrayproperty()
        {
            var result = new StringBuilder();

            var io = new IntellisenseObject(new[]
            {
                new IntellisenseProperty(_stringType, "AString"),
                new IntellisenseProperty(_int32Type, "AnInt"),
                new IntellisenseProperty(_int32ArrayType, "AnIntArray") { Summary = "ASummary"},
                new IntellisenseProperty(_simpleType, "TheSimple")
            })
            {
                FullName = "Foo.Primitives",
                Name = "Primitives",
                Namespace = "server"
            };
            IntellisenseWriter.WriteJavaScript(new[] { io }, result);

            result.ShouldBeCode(@"
var server = server || {};
/// <summary>The Primitives class as defined in Foo.Primitives</summary>
server.Primitives = function() {
/// <field name=""AString"" type=""String"">The AString property as defined in Foo.Primitives</field>
this.AString = '';
/// <field name=""AnInt"" type=""Number"">The AnInt property as defined in Foo.Primitives</field>
this.AnInt = 0;
/// <field name=""AnIntArray"" type=""Number[]"">ASummary</field>
this.AnIntArray = [];
/// <field name=""TheSimple"" type=""Object"">The TheSimple property as defined in Foo.Primitives</field>
this.TheSimple = { };
};");
        }

    }
}