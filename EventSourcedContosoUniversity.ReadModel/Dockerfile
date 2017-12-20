FROM microsoft/dotnet:2.0-runtime AS base
WORKDIR /app

FROM microsoft/dotnet:2.0-sdk AS build
WORKDIR /src
COPY *.sln ./
COPY EventSourcedContosoUniversity.ReadModel/EventSourcedContosoUniversity.ReadModel.csproj EventSourcedContosoUniversity.ReadModel/
COPY EventSourcedContosoUniversity.Core/EventSourcedContosoUniversity.Core.csproj EventSourcedContosoUniversity.Core/
RUN dotnet restore
COPY . .
WORKDIR /src/EventSourcedContosoUniversity.ReadModel
RUN dotnet build -c Release -o /app

FROM build AS publish
RUN dotnet publish -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "EventSourcedContosoUniversity.ReadModel.dll"]
