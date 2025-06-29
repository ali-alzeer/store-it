<br/>

## Table Of Contents

- [About The Project](#about-the-project)
- [Features](#features)
- [Screenshot](#screenshot)
- [Database Diagram](#database-diagram)
- [Live Site](#live-site)
- [Built With](#built-with)
- [Getting Started](#getting-started)
  - [Prerequisites](#prerequisites)
  - [Installation](#installation)

## About The Project

Online storage with full CRUD operations for files and users management, including file sharing functionality between users, with a beautiful UI that makes it easy for users to navigate through files, upload new ones, share them and other functionalities that every online storage has.

## Features

- User authentication using JWT
- Users capabilities to manage their own files
- Files searching and sorting
- Files sharing functionality
- User profile management

### Demo

[StoreIt Demo (YouTube)](https://youtu.be/8fT7nZ3oJ-Y?feature=shared)

### Screenshots

Main page

![Main page](https://res.cloudinary.com/alzeerecommerce/image/upload/v1750853802/Screenshot_2025-06-25_150932_aw94s9.png)

Sign-in / sign-up

![Sign-in / sign-up](https://res.cloudinary.com/alzeerecommerce/image/upload/v1750853804/Screenshot_2025-06-25_150459_r0hz6w.png)

Sharing functionality

![Sharing functionality](https://res.cloudinary.com/alzeerecommerce/image/upload/v1750853798/Screenshot_2025-06-25_151156_htblfj.png)

User details

![User details](https://res.cloudinary.com/alzeerecommerce/image/upload/v1750853802/Screenshot_2025-06-25_151234_y90oeu.png)

Responsive design

![Responsive design](https://res.cloudinary.com/alzeerecommerce/image/upload/v1750853802/Screenshot_2025-06-25_151322_al8zw7.png)

Responsive design (mobile)

![Responsive design](https://res.cloudinary.com/alzeerecommerce/image/upload/v1750853804/Screenshot_2025-06-25_151403_muvvjp.png)

<hr />

### Database Diagram

![db_diagram](https://res.cloudinary.com/alzeerecommerce/image/upload/v1750752745/drawing_ssr1xf.png)

## Built With

- Next.js
- ASP.NET Core
- SQL Server

## Getting Started

To get a local copy up and running follow these simple steps.

### Prerequisites

- Next.js 15
- .NET 9
- Microsoft SQL Server
- Cloudinary account

### Installation

#### Getting the code

1. Clone the repo

```sh
    git clone https://github.com/ali-alzeer/store-it.git
```

#### Database

1. Import the database file from the folder "storeitdatabase"

#### Back-End

1. Configure connection string in "appsettings.json" file

```json
    "ConnectionStrings": {
      "Default" : "YOUR_CONNECTION_STRING"
    }
```

2. Configure JWT settings in "appsettings.json" file

```json
    "Jwt": {
        "Key": "YOUR_JWT_KEY",
        "Issuer": "YOUR_JWT_ISSUER",
        "ExpiresInDays" : 15
    }
```

3. Configure Cloudinary settings in "appsettings.json" file

```json
    "Cloudinary": {
        "CloudName": "YOUR_CLOUDINARY_CLOUDNAME",
        "ApiKey": "YOUR_CLOUDINARY_API_KEY",
        "ApiSecret": "YOUR_CLOUDINARY_API_SECRET"
    }
```

4. Change path to the back-end folder

```sh
    cd storeitbackend
```

5. Install dependencies

```sh
    dotnet restore
```

6. Start Running

```sh
    dotnet run
```

#### Front-End

1. Create ".env.local" file and configure your BASEURL in it

```
    NEXT_PUBLIC_BACKEND_BASEURL = "YOUR_BACKEND_BASEURL"
```

2. Change path to the front-end folder

```sh
    cd storeitfrontend
```

3. Install dependencies

```sh
    npm install
```

4. Start Running

```sh
    npm run dev
```
