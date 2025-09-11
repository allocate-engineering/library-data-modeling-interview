FROM mcr.microsoft.com/dotnet/sdk:8.0

# Install PostgreSQL client, Java, and dependencies
RUN apt-get update && apt-get install -y \
    postgresql-client \
    openjdk-17-jre-headless \
    wget \
    curl \
    gnupg \
    && rm -rf /var/lib/apt/lists/*

# Install Liquibase using direct download (more reliable)
RUN wget -q https://github.com/liquibase/liquibase/releases/download/v4.28.0/liquibase-4.28.0.tar.gz \
    && tar -xzf liquibase-4.28.0.tar.gz -C /opt \
    && chmod +x /opt/liquibase \
    && ln -s /opt/liquibase /usr/local/bin/liquibase \
    && rm liquibase-4.28.0.tar.gz

# Download PostgreSQL JDBC driver
RUN wget -q https://jdbc.postgresql.org/download/postgresql-42.7.3.jar -O /opt/lib/postgresql.jar

WORKDIR /workspaces