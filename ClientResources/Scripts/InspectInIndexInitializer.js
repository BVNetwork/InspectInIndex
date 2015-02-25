define([
// Dojo
    "dojo",
    "dojo/_base/declare",
//CMS
    "epi/_Module",
    "epi/dependency",
    "epi/routes"
], function (
// Dojo
    dojo,
    declare,
//CMS
    _Module,
    dependency,
    routes
) {
    return declare([_Module], {
        initialize: function () {
            this.inherited(arguments);

            var registry = this.resolveDependency("epi.storeregistry");
            //Register the store
            registry.create("inspectinindexstore", this._getRestPath("inspectinindexstore"));
        },

        _getRestPath: function (name) {
            return routes.getRestPath({ moduleArea: "EPiCode.InspectInIndex", storeName: name });
        }
    });
});

