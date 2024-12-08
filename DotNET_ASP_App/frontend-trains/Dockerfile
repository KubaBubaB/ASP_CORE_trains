FROM node:16-alpine

WORKDIR /app

COPY package.json package-lock.json ./

RUN npm install

COPY . .

ENV PORT=8147

EXPOSE 8147

CMD ["npm", "start"]
