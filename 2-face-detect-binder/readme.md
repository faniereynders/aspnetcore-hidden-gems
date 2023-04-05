# Setup
To be able to run this example, you need an Azure subscription with a Microsoft Cognitive resource.

The access key is stored in the `user-secrets` local storage. Before running this example you need
to add the Microsoft Cognitive Services key to the local user secrets vault.

On the same directory as the csproj, run the following command:

```
dotnet user-secrets set SubscriptionKey <KEY>
```