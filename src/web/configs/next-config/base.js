/** @type {import('next').NextConfig} */
module.exports = {
  reactStrictMode: true,
  pageExtensions: ['ts', 'tsx', 'mdx'],
  transpilePackages: [
    'lucide-react',
    '@sisa/grpc-api',
    '@sisa/components',
    '@sisa/form',
    '@sisa/i18n',
    '@sisa/utils',
  ],
  modularizeImports: {
    '@mui/joy': {
      transform: '@mui/joy/{{member}}',
      preventFullImport: true,
    },
    '@sisa/components': {
      transform: '@sisa/components/src/{{kebabCase member}}',
      preventFullImport: true,
    },
    // '@sisa/form': {
    //   transform: '@sisa/form/src/{{kebabCase member}}',
    //   preventFullImport: true,
    // },
    '@sisa/hooks': {
      transform: '@sisa/hooks/src/{{kebabCase member}}',
      preventFullImport: true,
    },
    // '@sisa/i18n': {
    //   transform: '@sisa/i18n/src/{{kebabCase member}}',
    //   preventFullImport: true,
    // },
  },
  cleanDistDir: true,
  poweredByHeader: false,
  generateBuildId: () => {
    return new Promise((resolve, reject) => {
      require('child_process').exec('git rev-parse HEAD', function (err, stdout) {
        const hash = stdout.trim();
        const buildId = hash.slice(0, 7);

        console.log('Last commit hash on this branch is: ', hash);
        console.log('Using build id: ', buildId);

        if (err) {
          return reject(err);
        }

        return resolve(buildId);
      });
    });
  },
  experimental: {
    mdxRs: true,
    serverActions: true,
    typedRoutes: true,
    optimizePackageImports: ['@sisa/form', '@sisa/i18n'],
  },
};
