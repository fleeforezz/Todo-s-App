# 📑 Todo's App

We'll cover the following
+ [🎯 Project Overview](#🎯-project-overview)
+ [ 🛠️Technology Stack](#🛠️-technology-stack)
+ [🏗️ System Architecture](#🏗️-system-architecture)
+ [🎨 Database Design](#🎨-database-design)
+ [📚 Document References](#📚-document-references)

## 🎯 Project Overview
Todo’s is a lightweight and intuitive task management app designed to help you stay organized every day. With a clean interface and smooth workflow, you can create, edit, and track your tasks effortlessly — staying focused on what really matters.

A Windows desktop application built using WPF (Windows Presentation Foundation) with the MVVM (Model–View–ViewModel) architecture.

<img src="./Resources/logo.png" alt="logo">

### Github Workflow
<img src="./Resources/Github_Workflow.png" alt="Github workflow">

## 🛠️ Technology Stack
+ .NET 9.0 - Main Framework
+ WPF with MVVM architecture - UI
+ Entity Framework Core - ORM
+ MSSQL Server - Datbase
+ BCrypt.NET - Securely hashing and Verifying passwords

## 🏗️ System Architecture
```
[ WPF MVVM Application ]
   ↓
[ Database (MSSQL) ]
   ↓
[ File Storage (Local / S3 / MinIO) ]
```

## 🎨 Database Design
Our system will have total 3 Entities:

+ Users: User management and authorization
+ Todos: Store todos for each user
+ Category: Store category for each todo

## 📚 Document References
### Project Documentation
+ [⚙️ Project Main Flow](MAIN_FLOW.md)
+ [📰 Features](FEATURE.MD)
+ [🗄️ Database Overview](DATABASE.md)