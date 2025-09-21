A new major .NET version has been released, .NET 10 and you want to make sure the docs are updated to reflect this. Make sure that any reference to older .NET versions are updated to the latest major version. Examples of things to replace, but not limited to:

* Target framework mentions in code snippets like: net8.0-ios need to be updated to net10.0-ios
* File paths with the TFM in is, for example: bin\\Release\\net8.0-ios\\ios-arm64\\publish needs to be updated to bin\\Release\\net10.0-ios\\ios-arm64\\publish
* Textual mentions of .NET 8 need to be updated to .NET 10, however, if the text says just .NET without any version number, do not update those

Also be mindful of documentation where the version number is intentionally set to a certain version, for example when it's being talked about in the context of updating from one version to another.
