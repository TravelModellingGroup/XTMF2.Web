const path = require('path');

module.exports = {

  entry: './Areas/XTMF2.Web.Components.ts',
  mode: 'development',
  module: {
    rules: [
      {
        test: /\.tsx?$/,
        use: 'ts-loader',
        include: [ path.resolve(__dirname, "Areas")],
        exclude: /node_modules/
      }
    ]
  },
  
  resolve: {
    extensions: [ '.tsx', '.ts', '.js' ]
  },
  output: {
    library: "xtmf2WebComponents",
    filename: 'xtmf2.web.components.js',
    path: path.resolve(__dirname, 'wwwroot')
  }
};