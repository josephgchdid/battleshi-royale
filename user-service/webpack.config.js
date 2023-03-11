const path = require('path');
const nodeExternals = require('webpack-node-externals');

module.exports = { 

    target : 'node', 

    externals: [nodeExternals()],

    externalsPresets: {
        node: true // in order to ignore built-in modules like path, fs, etc. 
    },

    mode : "development",
    
    devtool : "inline-source-map",
    
    entry : {
        main : "./src/index.ts"
    },

    output: {
 
        path : path.resolve(__dirname, './out'),
      
        filename: "index.js", 
        
        library : {

            name : 'index',

            type :  'umd',
            
            export : 'default',

            umdNamedDefine : true
        },
        
        globalObject : "this"
    },

    resolve: {
        extensions: [  ".ts", ".tsx", ".js"]
    },

    module : {

        rules: [
            { 
              test: /\.tsx?$/,
              loader: "ts-loader",
              exclude: /node_modules/
            }
          ]
    }
};
