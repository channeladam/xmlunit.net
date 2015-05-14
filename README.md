XMLUnit.NET 2.x
===============

[![Build Status XMLUnit.NET 2.x](https://travis-ci.org/xmlunit/xmlunit.net.svg?branch=master)](https://travis-ci.org/xmlunit/xmlunit.net)

XMLUnit is a library that supports testing XML output in several ways.

Some goals for XMLUnit 2.x:

* create .NET and Java versions that are compatible in design while
  trying to be idiomatic for each platform
* remove all static configuration (the old XMLUnit class setter methods)
* focus on the parts that are useful for testing
  - XPath
  - (Schema) validation
  - comparisons
* be independent of any test framework

We are in the process of finalizing the API for XMLUnit 2.x.

## Help Wanted!

If you are looking for something to work on, we've compiled a
[list](https://github.com/xmlunit/xmlunit/blob/master/HELP_WANTED.md) of things that should be done before XMLUnit
2.0 can be released.

Please see the [contributing guide](CONTRIBUTING.md) for details on
how to contribute.

## Examples

These are some really small examples, more is to come in the [user guide](https://github.com/xmlunit/user-guide/wiki)

### Comparing Two Documents

```csharp
ISource control = Input.FromFile("test-data/good.xml").Build();
ISource test = Input.FromByteArray(CreateTestDocument()).Build();
IDifferenceEngine diff = new DOMDifferenceEngine();
diff.DifferenceListener += (comparison, outcome) => {
            Assert.Fail("found a difference: {}", comparison);
        };
diff.Compare(control, test);
```

or using the fluent builder API

```csharp
Diff d = DiffBuilder.Compare(Input.FromFile("test-data/good.xml"))
             .WithTest(CreateTestDocument()).Build();
Assert.IsFalse(d.HasDifferences());
```

or using the `CompareConstraint`

```csharp
Assert.That(CreateTestDocument(), CompareConstraint.IsIdenticalTo(Input.FromFile("test-data/good.xml")));
```

### Asserting an XPath Value

```csharp
ISource source = Input.FromString("<foo>bar</foo>").Build();
IXPathEngine xpath = new XPathEngine();
IEnumerable<XmlNode> allMatches = xpath.SelectNodes("/foo", source);
string content = xpath.evaluate("/foo/text()", source);
```

### Validating a Document Against an XML Schema

```csharp
Validator v = Validator.ForLanguage(Languages.W3C_XML_SCHEMA_NS_URI);
v.SchemaSources = new ISource[] {
        Input.FromUri("http://example.com/some.xsd").Build(),
        Input.FromFile("local.xsd").Build()
    };
ValidationResult result = v.ValidateInstance(Input.FromDocument(CreateDocument()).Build());
bool valid = result.Valid;
IEnumerable<ValidationProblem> problems = result.Problems;
```

or using `ValidationConstraint`

```csharp
Assert.That(CreateDocument(),
            new ValidationConstraint(Input.FromFile("local.xsd")));
```

## Requirements

XMLUnit requires .NET 3.5 (it is known to work and actually is
developed on Mono 4).

The `core` library provides all functionality needed to test XML
output and hasn't got any dependencies.  It uses NUnit 2.x for its own
tests.  The core library is complemented by NUnit constraints.

## Checking out XMLUnit.NET

XMLUnit.NET uses a git submodule for test resources it shares with
XMLUnit for Java.  You can either clone this repository using `git
clone --recursive` or run `git submodule update --init` after inside
your fresh working copy after cloning normally.

If you have checked out a working copy before we added the submodule,
you'll need to run `git submodule update --init` once.

## Building

XMLUnit for .NET uses NuGet and `msbuild`/`xbuild` - or Visual Studio.

When using Visual Studio the build should automatically refresh the NuGet packages, build the `core` and `constraints` assemblies as well as the unit test projects and run all NUnit tests.

When not using Visual Studio you need to [install nuget](http://docs.nuget.org/consume/installing-nuget) as well as `msbuild` or `xbuild` and run

```sh
$ nuget restore XMLUnit.NET.sln
```

once to download the packages used by XMLUnit during the build (really only NUnit right now).  After that you can run `msbuild` or `xbuild` like

```sh
> msbuild /p:Configuration=Debug XMLUnit.NET.sln
```
```sh
$ xbuild /p:Configuration=Debug XMLUnit.NET.sln
```

which compiles `core` and `constraints`, builds the assemblies and executes the NUnit tests.

### NAnt build

XMLUnit has used NAnt in the past and the build file is still in place
until we've taught one of our CI systems to use xbuild - don't rely on
it staying a supported option for building XMLUnit.
