define([
    "dojo",
    "dojo/parser",
    "dojo/_base/declare",
    "dojo/json",

    "dijit",
    "dijit/_WidgetBase",
    "dijit/_TemplatedMixin",
    "dijit/form/Button",

    "epi-cms/_ContentContextMixin",
    "epi-cms/contentediting/StandardToolbar",
    "epi/dependency"
],

function (
    dojo,
    parser,
    declare,
    json,

    dijit,
    _WidgetBase,
    _TemplatedMixin,
    Button,

    _ContentContextMixin,
    StandardToolbar,
    dependency
) {

    return declare([_WidgetBase, _TemplatedMixin, _ContentContextMixin], {
        templateString: '<div class="iii-inspectindexview"><div class="epi-localToolbar epi-viewHeaderContainer" data-dojo-attach-point="toolbarArea"/></div>' +
            '<div class="iii-findcontentView"><div class="iii-ButtonRow">' +
            '<button id="iiiIndexButton" type="button"></button>' +
            '<button data-dojo-attach-point="deleteButton" id="iiiDeleteButton" type="button"></button>' +
            '</div> <pre data-dojo-attach-point="findresult">Loading...</pre></div>',
        store: null,
        indexButton: null,
        postCreate: function () {
            this.inherited(arguments);
            this.toolbar = new StandardToolbar();
            this.toolbar.placeAt(this.toolbarArea, "first");
            this.store = dependency.resolve('epi.storeregistry').get('inspectinindexstore');
        },
        id: null,
        updateView: function (data, context) {
            this.findresult.innerHTML = "Loading...";
            this.id = context.id;
            this.toolbar.update({
                currentContext: context,
                viewConfigurations: {
                    availableViews: data.availableViews,
                    viewName: data.viewName
                }
            });
            var that = this;

            dojo.when(that.store.get(context.id), function (indexId) {
                that._populate(indexId);
            });
            Button({
                label: "Delete",
                disabled: true,
                class: "epi-danger",
                iconClass: "epi-iconTrash epi-icon--inverted",
                onClick: function () {
                    dojo.when(that.store.remove(that.id), function () {
                        that.findresult.innerHTML = 'Deleted from index.';
                        dijit.byId("iiiDeleteButton").set("disabled", true);
                        dijit.byId("iiiIndexButton").set("label", "Index");
                    });
                }
            },
            "iiiDeleteButton");

            Button({
                label: "Index",
                disabled: true,
                class: "epi-primary",
                iconClass: "epi-iconReload epi-icon--inverted",
                onClick: function () {
                    that.findresult.innerHTML = "Loading...";
                    dojo.when(that.store.put({
                        reference: that.id,
                    }), function (indexId) {
                        that._populate(indexId);
                    });
                }
            },
            "iiiIndexButton");
        },

        _populate: function (data) {
            var targetNode = this.findresult;
            var indexBtn = dijit.byId("iiiIndexButton");
            var xhrArgs = {
                url: data.path,
                handleAs: "json",
                load: function (indexData) {
                    //// Replace tabs with spaces.
                    targetNode.innerHTML = json.stringify(indexData._source, null, 4);
                    dijit.byId("iiiDeleteButton").set("disabled", false);

                    indexBtn.set("disabled", false);
                    indexBtn.set("label", "Reindex");
                },
                error: function (error) {
                    if (error.status == 404) {
                        indexBtn.set("disabled", false);
                        indexBtn.set("label", "Index");
                        targetNode.innerHTML = "Content was not found in index.";
                    } else {
                        targetNode.innerHTML = "An unexpected error occurred: " + error;
                    }
                }
            };
            dojo.xhrGet(xhrArgs);
        }

    });
});