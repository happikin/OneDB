# OneDBv2.0

Hello! This is my first attempt at building a complete project using .net,
I am using VB.NET not because I like it but mainly because I wanted to make use of what is being taught at my University.

## What OneDB is supposed to achieve?

using this application a user can navingate through as many tables in a fixed database: thus named OneDB.
However the application is turning towards a multiDB functoinality very quickly!
This DataBase(MySQL) handling system is mainly for small data jobs like for a college or a uni.
The custom queries from the GUI become little complicated and go way beyond what a user would like to have,
thus i have kept it very usable and simple for the above mentioned use cases.
 ## Requirements to use OneDB
  I. There are two .txt files in the OneDB Debug folder which are not to me moved anywhere else.
    in the credentials.txt file write the database name where all username/password are stored
    in the database.txt file write the database name which you want to access through the OneDB GUI Application
    step 1: create database authdetails;
    step 2: create table onedb(username char(50) NOTNULL,password char(50) NOTNULL);
    This wil be enough to make OneDB go boom on your computer!
  II. Note : I developed this application on a 14 inch laptop with 150%(recommended) zoom setting,
             so if it malfunctions onyour computer then do the above adjustments or help imporove the code.
