const path = require('path');

const config = require('./base');

/** @type {import('next').NextConfig} */
module.exports = {
  ...config,
  output: 'standalone',
  experimental: {
    ...config.experimental,
    mdxRs: true,
    serverActions: true,
    typedRoutes: true,
    outputFileTracingRoot: path.join(__dirname, '../../'),
  },
};
