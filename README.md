# Freya - Full business example (.NET MAUI - WPF - ASP NET)

## This software is an example but is ready to run in production.

### This repository contains a series of projects that represent all the software of a fictitious company "Software as a Service" (SaaS).
### The business target is pregnancy clinics. Our 'company' is called Freya and sells software to clinics. The software supports one clinic or companies with multiple clinics. The software allows the clinics to manage their clients and the images, videos and sounds of the clients. The clinics offer a mobile app for their clients. 
### In addition we have a software to manage all clinics from Freya. 
### Clinics have to pay a subscription fee to use the software.

## The software has the following solutions.

| Name | Technology | Description | Solution | CI | CD |
| --- | --- | --- | --- | --- | --- |
| Freya Mobile App | .NET MAUI | Mobile app |![Build status](https://github.com/migueBarrera)|![Build status](https://github.com/migueBarrera)|![Build status](https://github.com/migueBarrera)|
| Freya Client | WPF | Mobile app |![Build status](https://github.com/migueBarrera)|![Build status](https://github.com/migueBarrera)|![Build status](https://github.com/migueBarrera)|
| Freya Manager | WPF | Mobile app| ![Build status](https://github.com/migueBarrera)|![Build status](https://github.com/migueBarrera)|![Build status](https://github.com/migueBarrera)|
| Freya API | ASP.NET | Mobile app |![Build status](https://github.com/migueBarrera)|![Build status](https://github.com/migueBarrera)|![Build status](https://github.com/migueBarrera)|



## Technologies
* All apps (Server, Manager, Client and Mobile) are made with [.NET 7](https://docs.microsoft.com/es-es/dotnet/maui/what-is-maui)
* .NET MAUI implements IOS and Android.
* Backend uses Asp .NET7 Api with MySql database
* Data access is done with Entity Framework Core and models are shared in all projects.
* Stripe is used for payments
------