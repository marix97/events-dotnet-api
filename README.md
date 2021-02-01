# Web API for events-guests relationship.
## Targeted Framework .NET Core 3.1

### Overview

The API endpoints are accessible only through the API key that is in appsettings file.
Upon creation of an event, an email is sent to the guests that are "subscribed" to that event with a calendar file attached to the email that describes the event.
You can extend the Email Service and send emails to inform the guests when an event is deleted or updated. 
