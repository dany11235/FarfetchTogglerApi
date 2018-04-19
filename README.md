# FarfetchTogglerApi

# Toggler API Using ASP.NET Web API

Patterns
<ul>
<li>Repository pattern</li>
<li>Unit of work pattern</li>
<li>Dependency Injection</li>
</ul>

Features used

<ul>
<li>Entity Framework</li>
<li>Unity</li>
<li>MEF</li>
</ul>


<h3>Installation Instructions </h3>

<ul>
<li>Install SqlServer Express</li>
<li>Create a database and run the database script (Already contains some data)</li>
<li>Open the solution on visual studio and change the connection string according</li>
<li>Run the solution</li>
<li>The Api Documentation will be under localhost:someport/help</li>
<li>Use e.g Postman to call API endpoints</li>
</ul>

<h3>Authentication </h3>

<ul>
<li>Call the api endpoint 'api/Authentication' with Basic Authentication (Username:Admin , password:Admin)</li>
<li>A token will be sent to you in the response headers</li>
<li>Use that token to make requests that require authentication sending it in request headers.(Header parameter: Token)</li>
</ul>


<h3>Improvements </h3>
<ul>
<li>Unit and integration tests (Using NUnit and Moq Framework)</li>

</ul>



