
Advanced Stats Page

=============

This tool allows users to visualize Sitecore rendering stats data in an intuitive and simplified way. Which will help you to identify slow renderings, fix them and have happy end users!.
**Version 1.0**
![Advanced Stats Page](https://sitecorebasics.files.wordpress.com/2015/11/statsv2-firstcut.gif  "Sitecore Advanced Stats Page")
**Version 2.1**
![enter image description here](https://sitecorebasics.files.wordpress.com/2021/08/stats-v21.gif)
 ##Main Features 
1. Simplified version of Sitecore Stats.aspx page
2. You can filter, sort, paginate and search records
3. Data Visualize your renderings : Plots chart of top 10 slowest renderings by time taken
4. Security has been applied -- User needs to be logged in to access it. (Refer Version 2.1 Change log)
 

##How to Download and Install?
  

### Option 1

1. If you would like to do it manually you can download files from here
2. Create folder named as "stats" under <WEBROOT>\Sitecore\tools copy all files to this folder.
3. Access your page using http://<YOURHOSTNAME>/tools/stats/stats.aspx
4. That's it! Enjoy! :-)  
***
### Option 2
1. Download Sitecore Package from Packages\V2.1 folder, Which will copy the files at required place.
2. Install using Sitecore Installation Wizard. Once done, Just access your page using http://<YOURHOSTNAME>/tools/stats/stats.aspx
3. That's it! Enjoy! :-)

>Found any bug? Got suggestion/feedback/comment, Share it here!

**Version History**
| Version | Release Notes  |
|--|--|
| 1.0 | Initial version |
| 2.1 | -> Tool security has been removed as Sitecore/Admin folder is generally disabled on CD Server. So, please delete this tool once you are done. ->Visual indication to indicate whether a component is being rendered from HTML Cache or not|
