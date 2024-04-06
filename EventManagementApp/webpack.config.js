const path = require("path");
const Webpack = require("webpack");
const CopyWebpackPlugin = require("copy-webpack-plugin");
module.exports = {
  entry: {
    "test.js": "./wwwroot/src/ts/test.ts",
  },
  mode: "development",
  output: {
    filename: "[name]",
    path: path.resolve(__dirname, "wwwroot/dist/js"),
  },
  resolve: {
    alias: {
      vue: "vue/dist/vue.esm-bundler.js",
    },
    extensions: [".js", ".ts"],
  },
  plugins: [
    new Webpack.DefinePlugin({
      __VUE_OPTIONS_API__: true,
      __VUE_PROD_DEVTOOLS__: true,
    }),
    new CopyWebpackPlugin({
      patterns: [
        {
          from: path.resolve(__dirname, "wwwroot/src/js/no-build"),
          to: path.resolve(__dirname, "wwwroot/dist/js"),
        },
        {
          from: path.resolve(__dirname, "wwwroot/src/css/no-build"),
          to: path.resolve(__dirname, "wwwroot/dist/css"),
        },
        {
          from: path.resolve(__dirname, "wwwroot/src/lib"),
          to: path.resolve(__dirname, "wwwroot/dist/lib"),
        },
      ],
    }),
  ],
  module: {
    rules: [
      {
        test: /\.css$/i,
        use: ["style-loader", "css-loader"],
      },
      {
        test: /\.ts?$/,
        use: "ts-loader",
        exclude: /node_modules/,
      },
    ],
  },
};
