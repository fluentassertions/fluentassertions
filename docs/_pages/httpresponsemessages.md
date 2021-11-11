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
successfulResponse.Should().BeSuccessful("it's set to OK"); // (HttpStatusCode = 2xx)

var redirectResponse = new HttpResponseMessage(HttpStatusCode.Moved);
redirectResponse.Should().BeRedirection("it's set to Moved"); // (HttpStatusCode = 3xx)

var clientErrorResponse = new HttpResponseMessage(HttpStatusCode.BadRequest);
clientErrorResponse.Should().HaveClientError("it's set to BadRequest"); // (HttpStatusCode = 4xx)
clientErrorResponse.Should().HaveError("it's set to BadRequest"); // (HttpStatusCode = 4xx or 5xx)

var serverErrorResponse = new HttpResponseMessage(HttpStatusCode.InternalServerError);
serverErrorResponse.Should().HaveServerError("it's set to InternalServerError"); // (HttpStatusCode = 5xx)
serverErrorResponse.Should().HaveError("it's set to InternalServerError"); // (HttpStatusCode = 4xx or 5xx)

var anotherResponse = new HttpResponseMessage(HttpStatusCode.Moved);
anotherResponse.Should().HaveStatusCode(HttpStatusCode.Moved);
anotherResponse.Should().NotHaveStatusCode(HttpStatusCode.OK);
```
