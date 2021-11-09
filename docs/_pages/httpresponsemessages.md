---
title: HttpResponseMessages
permalink: /httpresponsemessages/
layout: single
classes: wide
sidebar:
nav: "sidebar"
---

```csharp
var successfulResponse = new HttpResponseMessage(HttpStatusCode.OK);
successfulResponse.Should().BeSuccessful("it's set to OK");

var redirectResponse = new HttpResponseMessage(HttpStatusCode.Moved);
redirectResponse.Should().BeRedirection("it's set to Moved");

var clientErrorResponse = new HttpResponseMessage(HttpStatusCode.BadRequest);
clientErrorResponse.Should().BeClientError("it's set to BadRequest");
clientErrorResponse.Should().BeError("it's set to BadRequest");

var serverErrorResponse = new HttpResponseMessage(HttpStatusCode.InternalServerError);
serverErrorResponse.Should().BeServerError("it's set to InternalServerError");
serverErrorResponse.Should().BeError("it's set to InternalServerError");

var anotherResponse = new HttpResponseMessage(HttpStatusCode.Moved);
anotherResponse.Should().HaveStatusCode(HttpStatusCode.Moved);
anotherResponse.Should().NotHaveStatusCode(HttpStatusCode.OK);
```
