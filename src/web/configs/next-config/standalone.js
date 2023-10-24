const path = require('path');

const config = require('./base');

/** @type {import('next').NextConfig} */
module.exports = {
  ...config,
  output: 'standalone',
  experimental: {
    ...config.experimental,
    outputFileTracingRoot: path.join(__dirname, '../../'),
  },
};
