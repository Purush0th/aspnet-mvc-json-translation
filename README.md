# JSON Translation : Asp.Net Framework

This .NET Framework MVC project that demonstrates how to implement JSON-based translation. Unlike traditional RESX files, JSON-based translation offers a flexible and human-readable format for managing translations. It allows for easy collaboration with non-developers and supports dynamic updates without requiring redeployment.

## Table of Contents

- [Introduction](#introduction)
- [Features](#features)
- [Getting Started](#getting-started)
  - [Prerequisites](#prerequisites)
- [Usage](#usage)
- [Configuration](#configuration)

## Introduction

This project showcases a .NET Framework web application that utilizes JSON files for translation instead of traditional RESX files. It demonstrates how to set up a TranslationService, handle dynamic culture changes, and use translations in both controllers and views.

## Features

- JSON-based translation for multiple locales.
- Dynamic culture changes without redeployment.
- Static class can be used in controller or views.

## Getting Started

### Prerequisites

- .NET Framework 4.8.1
- Visual Studio

## Usage
```csharp
public class HomeController : BaseController
{
    public ActionResult Index()
    {
        ViewBag.Message = TranslateUtil.Translate("APP_TEXT_WELCOME");
        return View();
    }
}
```
```cshtml
<main>
    <section class="row" aria-labelledby="aspnetTitle">
        <h1 id="title">@TranslateUtil.Translate("MAIN_GREET_TEXT", "fr-FR")</h1>
</section>
</main>
```
## Configuration
```
<configuration>
  <appSettings>
    <add key="Translation:DefaultLanguage" value="en-US"/>
    <add key="Translation:JsonFolderPath" value="C:\Dev\ScratchBox\json-translation\translation\JsonExport"/>
  </appSettings>
```
