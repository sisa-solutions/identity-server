module.exports = {
  root: true,
  // This tells ESLint to load the config from the package `@sisa/eslint-config`
  extends: ["@sisa/eslint-config"],
  settings: {
    next: {
      rootDir: ["apps/*/"],
    },
  },
};
