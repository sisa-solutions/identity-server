const jsonServer = require('json-server');
const server = jsonServer.create();
const router = jsonServer.router('db.json');
const middlewares = jsonServer.defaults();

// Set default middlewares (logger, static, cors and no-cache)
server.use(middlewares);

// To handle POST, PUT and PATCH you need to use a body-parser
// You can use the one used by JSON Server
server.use(jsonServer.bodyParser);

server.use((req, res, next) => {
  res.header('Access-Control-Allow-Origin', '*');
  res.header('Access-Control-Allow-Headers', '*');

  next();
});

// Rewrite routes
server.use(
  jsonServer.rewriter({
    '/blog/api/v1/*': '/$1',
  })
);

// Use default router
server.use(router);

server.listen(process.env.PORT ?? 12000, () => {
  // log the status of server and full url
  console.log('JSON Server is running');
  console.log(`http://localhost:${process.env.PORT ?? 12000}`);
});
