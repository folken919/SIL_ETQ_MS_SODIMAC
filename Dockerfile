#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:3.1 AS base
WORKDIR /app
#init Openshift
LABEL io.k8s.display-name="etq-impresion" \
      io.k8s.description="Web api impresion etq" \
      io.openshift.expose-services="8080:http"

EXPOSE 8080
ENV ASPNETCORE_URLS=http://*:8080
ENV TZ=America/Bogota
ENV LANG es_ES.UTF-8
ENV LANGUAGE ${LANG}
ENV LC_ALL ${LANG}
#end Openshift

#library
#RUN apt-get -qq -y install iputils-ping
RUN apt-get update
RUN apt-get install -y iputils-ping
#RUN ./bin/skopeo --version
#end library

FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build
WORKDIR /src
COPY ["Sodimac.Cedis.Api/Sodimac.Cedis.Api.csproj", "Sodimac.Cedis.Api/"]
RUN dotnet restore "Sodimac.Cedis.Api/Sodimac.Cedis.Api.csproj"
COPY . .
WORKDIR "/src/Sodimac.Cedis.Api"
RUN dotnet build "Sodimac.Cedis.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Sodimac.Cedis.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet","Sodimac.Cedis.Api.dll"]
