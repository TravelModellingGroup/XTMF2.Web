# Building XTMF2.Web.Components

## Requirements

Node and NPM must be installed. 

## Building

Run `npm install` at least once before the first build to restore the node package dependencies.

After the node packages are available, regular dotnet builds are enough to build the project library. All source typescript files are compiled and bundled into a single output file in wwwroot/.

`dotnet build` to build the project (standard).