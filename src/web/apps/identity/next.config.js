const { staticExport } = require('@sisa/next-config');

/** @type {import('next').NextConfig} */
module.exports = {
  ...staticExport,
  // trick for ASP.NET SPA Proxy
  webpack: (config, { dev }) => {
    if (dev) {
      console.log('Starting the development server');
    }

    return config;
  }
};
