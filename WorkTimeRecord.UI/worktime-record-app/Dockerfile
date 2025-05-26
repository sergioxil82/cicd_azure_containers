FROM node:lts-alpine AS build-stage
WORKDIR /app
COPY package*.json ./
RUN npm install
COPY . .

ARG MODE

# Copia el archivo de entorno adecuado a .env
RUN cp .env.${MODE} .env

RUN ls -l .env* 
RUN cat .env

RUN npm run build

FROM nginx:stable-alpine AS production-stage

COPY --from=build-stage /app/dist /usr/share/nginx/html

COPY nginx.conf /etc/nginx/conf.d/default.conf

EXPOSE 80

CMD ["nginx", "-g", "daemon off;"]
