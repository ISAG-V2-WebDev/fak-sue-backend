FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
ENV PORT 3000
WORKDIR /app
EXPOSE $PORT

ENV ASPNETCORE_URLS=http://+:${PORT}
ENV CONNECTION_STRING="mongodb+srv://Guest:letYouGuest@foods.g9ydd2t.mongodb.net/Foods?retryWrites=true&w=majority"
ENV ADMIN_CONNECTION_STRING="mongodb+srv://Admin:P4ssw0rD@foods.g9ydd2t.mongodb.net/Foods?retryWrites=true&w=majority"
ENV MongoDB__ConnectionString="mongodb+srv://Guest:letYouGuest@foods.g9ydd2t.mongodb.net/Foods?retryWrites=true&w=majority"
ENV MongoDB__DatabaseName="UsersDB"
ENV MongoDB__UsersCollectionName="Users"
ENV MongoDB__BlogsCollectionName="Orders"
ENV Hmac__Key="(:@},!<FCwng[T^go^OHH?ZB;h_8,[:?"
ENV Hmac__SecretKey="Q,o[aa}YyFHcr@;hzQQ;nJPq>Wz>@I'0"
ENV Jwt__SecretKey="uW7AGfPkDlS7KQMN7n7AFD65fjqWxO4tzPe_oeNr3wwtJI4U1Fxx21zzjwJVmF9cVIMMpYn6oiqd3rgliKy4pBdeNFJFNAUgRO7bUW8RtG4Y4qNMQUCFNr0Zf5oAucxgtfIBLM7GSMl_rg2nZjevjF6NQIXdQ74s5VYA3Zwctt0"
ENV Jwt__Audience="*"
ENV Authorization__PolicyName="RequiredAdmin"
ENV Authorization__Admin="admin"
ENV Cors__PolicyName="AllowOrigins"
ENV Cors__AllowOrigins="http://localhost:5500","http://localhost:3000"

RUN adduser -u 5678 --disabled-password --gecos "" appuser && chown -R appuser /app
USER appuser

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Backend.csproj", "./"]
COPY . .
WORKDIR "/src/."
RUN dotnet build "Backend.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Backend.csproj" -c Release -o /app/publish /p:UseAppHost=false 

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "Backend.dll"]