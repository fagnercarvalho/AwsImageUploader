# AWS Image Resizer

This repository has two applications:

- One console application responsible for uploading images to an Amazon S3 bucket;
- A Amazon Lambda service responsible for returning the uploaded images in any desired resolution.

This project was made for a client and delivered in April 2019.

## Prerequisites

To build and run this project you are going to need:

- .NET Framework 4.7.2;
- .NET Core 2.1;
- Visual Studio 2019;
- AWS Toolkit for Visual Studio (used to push the Lambda service to AWS).

## Example

This project has a test bucket containing 1 image that can be resized using the Lambda service.

[Here](https://s3-sa-east-1.amazonaws.com/test-bucket-henry-upwork/tokyo.jpg) is the link for the image.

To resize this image access the following address setting the resolution query parameter in the format width x height: https://6hkd95oxl9.execute-api.us-east-2.amazonaws.com/default/ImageResizer?keyName=tokyo.jpg&resolution=800x600.
