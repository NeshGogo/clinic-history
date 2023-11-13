# Stage 1: Build Angular App
FROM node:latest as build
WORKDIR /app
COPY package*.json ./
RUN npm install
RUN npx ngcc --properties es2023 browser module main --first-only --create-ivy-entry-points
COPY . .
RUN npm run build


# Stage 2: Serve Angular App with Nginx
FROM nginx:stable

# Copy the built Angular app from the previous stage
COPY --from=build /app/dist/web-app/ /usr/share/nginx/html

# Expose port 80
EXPOSE 80
FROM nginx:stable
COPY --from=build /app/dist/web-app/ /usr/share/nginx/html
EXPOSE 80 

# Copy Nginx configuration
COPY nginx.conf /etc/nginx/nginx.conf