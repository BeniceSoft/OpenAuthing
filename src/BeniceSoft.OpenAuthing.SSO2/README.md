# OpenAuthing SSO

This repo is based on the Tailwind CSS、Blazor and Flowbite

## Getting started

Ensure that you have NPM and Node.js installed on your project. Also ensure you have installed the .NET SDK to enable you to develop .NET
applications. Run the following command to install all dependencies:

```bash
npm install
```

Then run this command to compile the source code and watch for changes:

```bash
dotnet watch
```

Make sure that you also run the following script to compile the Tailwind CSS source code:

```bash
npx tailwindcss -i wwwroot/css/app.css -o wwwroot/css/app.min.css --watch
```

Run this command to build your project and all its dependencies:

```bash
dotnet build
```