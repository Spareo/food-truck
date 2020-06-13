FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-bionic AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-bionic AS build
ARG CONFIGURATION=Release

COPY . .

RUN dotnet restore src/FoodTruck.sln
WORKDIR /src/FoodTruck.Web

# Create the home directory for the new app user.
RUN mkdir -p /home/app
RUN dotnet build FoodTruck.Web.csproj -c ${CONFIGURATION} -o /app

FROM build AS publish
ARG CONFIGURATION=Release
RUN dotnet publish FoodTruck.Web.csproj -c ${CONFIGURATION} -o /app

FROM base AS final
ARG CONFIGURATION=Release

# Create an app user so our program doesn't run as root.
RUN groupadd -g 3000 -r app &&\
    useradd -u 3000 -r -g app -d /home/app -s /sbin/nologin -c "Service user account" app

# Set the home directory to our app user's home.
ENV HOME=/home/app
ENV APP_HOME=/home/app/service

# SETTING UP THE APP #
RUN mkdir -p ${APP_HOME}
WORKDIR ${APP_HOME}

COPY --from=publish --chown=app:app /app ${APP_HOME}

# Conditionally install remote debug tools if CONFIGURATION is set to Debug
RUN if [ "$CONFIGURATION" = "Debug" ] ; then apt-get update && apt-get install -y --no-install-recommends unzip && apt-get install -y procps && rm -rf /var/lib/apt/lists/* && curl -sSL https://aka.ms/getvsdbgsh | bash /dev/stdin -v latest -l /vsdbg; fi

# Change to the app user.
USER app

ENTRYPOINT ["dotnet", "FoodTruck.Web.dll"]