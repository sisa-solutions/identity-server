const { staticExport } = require('@sisa/next-config');

/** @type {import('next').NextConfig} */
module.exports = {
  ...staticExport,
  // trick for ASP.NET SPA Proxy, see: https://github.com/dotnet/aspnetcore/blob/6c1b3dfb66a1b66cc32ee26bbc23f6472f1dc985/src/Middleware/Spa/SpaServices.Extensions/src/ReactDevelopmentServer/ReactDevelopmentServerMiddleware.cs#L74C19-L74C20
  webpack: (config, { dev }) => {
    if (dev) {
      console.log('Starting the development server');
    }

    return config;
  }
};
