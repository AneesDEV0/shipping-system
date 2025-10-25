// roadmap.js - GoJS Learning Roadmap Builder
// Place this file in wwwroot/js/roadmap.js

let myDiagram = null;
let nodeIdCounter = 1;

// Initialize the GoJS diagram
function initRoadmap() {
    const $ = go.GraphObject.make;

    myDiagram = $(go.Diagram, "roadmapDiagram", {
        "undoManager.isEnabled": true,
        "clickCreatingTool.archetypeNodeData": { text: "New Block", color: "lightblue" },
        "toolManager.mouseWheelBehavior": go.ToolManager.WheelZoom,
        allowDrop: true,
        "LinkDrawn": showLinkLabel,
        "LinkRelinked": showLinkLabel,
        "animationManager.duration": 800,
        "undoManager.isEnabled": true
    });

    // Define node template
    myDiagram.nodeTemplate =
        $(go.Node, "Auto",
            {
                locationSpot: go.Spot.Center,
                isShadowed: true,
                shadowBlur: 1,
                shadowOffset: new go.Point(0, 1),
                shadowColor: "rgba(0, 0, 0, .14)"
            },
            new go.Binding("location", "location", go.Point.parse).makeTwoWay(go.Point.stringify),
            // Shape binding
            $(go.Shape, "RoundedRectangle",
                {
                    name: "SHAPE",
                    fill: "lightblue",
                    strokeWidth: 0,
                    portId: "",
                    fromLinkable: true,
                    toLinkable: true,
                    cursor: "pointer"
                },
                new go.Binding("fill", "color"),
                new go.Binding("figure", "shape")
            ),
            $(go.TextBlock,
                {
                    font: "bold 14px Lato, sans-serif",
                    margin: 12,
                    maxSize: new go.Size(200, NaN),
                    wrap: go.TextBlock.WrapFit,
                    editable: true,
                    stroke: "white"
                },
                new go.Binding("text").makeTwoWay()),
            // Context menu
            {
                contextMenu: $(go.Adornment, "Vertical",
                    $("ContextMenuButton",
                        $(go.TextBlock, "Edit Text"),
                        { click: editText }),
                    $("ContextMenuButton",
                        $(go.TextBlock, "Change Color"),
                        { click: changeColor }),
                    $("ContextMenuButton",
                        $(go.TextBlock, "Change Shape"),
                        { click: changeShape }),
                    $("ContextMenuButton",
                        $(go.TextBlock, "Set URL"),
                        { click: setURL }),
                    $("ContextMenuButton",
                        $(go.TextBlock, "Delete"),
                        { click: deleteNode })
                ),
                click: function (e, node) {
                    if (node.data.url) {
                        window.open(node.data.url, '_blank');
                    }
                },
                toolTip: $(go.Adornment, "Auto",
                    $(go.Shape, { fill: "#FFFFCC" }),
                    $(go.TextBlock, { margin: 4 },
                        new go.Binding("text", "", function (data) {
                            let tip = data.text;
                            if (data.url) {
                                tip += "\n🔗 " + data.url;
                            }
                            return tip;
                        }))
                )
            }
        );

    // Define link template
    myDiagram.linkTemplate =
        $(go.Link,
            {
                routing: go.Link.AvoidsNodes,
                curve: go.Link.JumpOver,
                corner: 5,
                toShortLength: 4,
                relinkableFrom: true,
                relinkableTo: true,
                reshapable: true,
                resegmentable: true,
                mouseEnter: function (e, link) { link.findObject("HIGHLIGHT").stroke = "rgba(30,144,255,0.2)"; },
                mouseLeave: function (e, link) { link.findObject("HIGHLIGHT").stroke = "transparent"; },
                selectionAdorned: false
            },
            new go.Binding("points").makeTwoWay(),
            $(go.Shape,
                {
                    name: "HIGHLIGHT",
                    strokeWidth: 5,
                    stroke: "transparent"
                }),
            $(go.Shape,
                {
                    strokeWidth: 3,
                    stroke: "gray"
                },
                new go.Binding("stroke", "color")),
            $(go.Shape,
                {
                    toArrow: "standard",
                    strokeWidth: 0,
                    fill: "gray"
                },
                new go.Binding("fill", "color")),
            $(go.Panel, "Auto",
                { visible: false },
                new go.Binding("visible", "visible"),
                $(go.Shape, "RoundedRectangle",
                    { fill: "#F8F8F8", strokeWidth: 0 }),
                $(go.TextBlock,
                    {
                        textAlign: "center",
                        font: "10pt helvetica, arial, sans-serif",
                        stroke: "#333333",
                        margin: 2,
                        minSize: new go.Size(10, NaN),
                        editable: true
                    },
                    new go.Binding("text").makeTwoWay())
            )
        );

    // Initialize with empty model
    myDiagram.model = new go.GraphLinksModel([], []);

    // Enable delete key
    myDiagram.commandHandler.deleteSelection = function () {
        deleteSelected();
    };

    return true;
}

// Add a new block to the diagram
function addBlock(text, color, url, shape) {
    if (!myDiagram) return false;

    const nodeData = {
        key: nodeIdCounter++,
        text: text || "New Block",
        color: color || "lightblue",
        shape: shape || "RoundedRectangle",
        location: Math.random() * 500 + " " + Math.random() * 300
    };

    if (url) {
        nodeData.url = url;
    }

    myDiagram.model.addNodeData(nodeData);
    return true;
}

// Delete selected nodes and links
function deleteSelected() {
    if (!myDiagram) return false;

    myDiagram.commandHandler.deleteSelection();
    return true;
}

// Save roadmap to LocalStorage and return JSON
function saveRoadmap(download) {
    if (!myDiagram) return null;

    const json = myDiagram.model.toJson();

    // Save to LocalStorage
    localStorage.setItem('roadmapData', json);

    // If download requested, trigger file download
    if (download) {
        const blob = new Blob([json], { type: 'application/json' });
        const url = URL.createObjectURL(blob);
        const a = document.createElement('a');
        a.href = url;
        a.download = 'roadmap.json';
        document.body.appendChild(a);
        a.click();
        document.body.removeChild(a);
        URL.revokeObjectURL(url);
    }

    return json;
}

// Load roadmap from JSON string
function loadRoadmap(jsonString) {
    if (!myDiagram) return false;

    try {
        myDiagram.model = go.Model.fromJson(jsonString);

        // Update nodeIdCounter to avoid conflicts
        const nodes = myDiagram.model.nodeDataArray;
        if (nodes && nodes.length > 0) {
            const maxKey = Math.max(...nodes.map(n => n.key || 0));
            nodeIdCounter = maxKey + 1;
        }

        return true;
    } catch (e) {
        console.error('Error loading roadmap:', e);
        return false;
    }
}

// Load roadmap from LocalStorage
function loadFromLocalStorage() {
    const json = localStorage.getItem('roadmapData');
    if (json) {
        return loadRoadmap(json);
    }
    return false;
}

// Load roadmap from file
function loadFromFile() {
    const input = document.createElement('input');
    input.type = 'file';
    input.accept = '.json';

    input.onchange = function (e) {
        const file = e.target.files[0];
        if (file) {
            const reader = new FileReader();
            reader.onload = function (event) {
                loadRoadmap(event.target.result);
            };
            reader.readAsText(file);
        }
    };

    input.click();
}

// Helper functions for context menu
function editText(e, obj) {
    const node = obj.part.adornedPart;
    if (node) {
        const newText = prompt("Enter new text:", node.data.text);
        if (newText !== null) {
            myDiagram.model.setDataProperty(node.data, "text", newText);
        }
    }
}

function changeColor(e, obj) {
    const node = obj.part.adornedPart;
    if (node) {
        const colors = ["lightblue", "lightgreen", "lightyellow", "lightcoral", "lightpink", "lightsalmon", "lightgray", "lightsteelblue"];
        const currentIndex = colors.indexOf(node.data.color);
        const newColor = colors[(currentIndex + 1) % colors.length];
        myDiagram.model.setDataProperty(node.data, "color", newColor);
    }
}

function changeShape(e, obj) {
    const node = obj.part.adornedPart;
    if (node) {
        const shapes = ["RoundedRectangle", "Rectangle", "Circle", "Diamond", "Triangle", "Hexagon"];
        const currentIndex = shapes.indexOf(node.data.shape || "RoundedRectangle");
        const newShape = shapes[(currentIndex + 1) % shapes.length];
        myDiagram.model.setDataProperty(node.data, "shape", newShape);
    }
}

function setURL(e, obj) {
    const node = obj.part.adornedPart;
    if (node) {
        const newURL = prompt("Enter URL (leave empty to remove):", node.data.url || "");
        if (newURL !== null) {
            if (newURL.trim() === "") {
                myDiagram.model.setDataProperty(node.data, "url", undefined);
            } else {
                myDiagram.model.setDataProperty(node.data, "url", newURL);
            }
        }
    }
}

function deleteNode(e, obj) {
    const node = obj.part.adornedPart;
    if (node) {
        myDiagram.model.removeNodeData(node.data);
    }
}

function showLinkLabel(e) {
    const label = e.subject.findObject("LABEL");
    if (label !== null) label.visible = (e.subject.fromNode.data.category === "Conditional");
}

// Get all shapes available
function getAvailableShapes() {
    return ["RoundedRectangle", "Rectangle", "Circle", "Diamond", "Triangle", "Hexagon"];
}

// Get all colors available
function getAvailableColors() {
    return ["lightblue", "lightgreen", "lightyellow", "lightcoral", "lightpink", "lightsalmon", "lightgray", "lightsteelblue", "lavender", "peachpuff"];
}

// Clear the entire diagram
function clearDiagram() {
    if (!myDiagram) return false;

    myDiagram.model = new go.GraphLinksModel([], []);
    nodeIdCounter = 1;
    return true;
}

// Export functions for Blazor
window.RoadmapJS = {
    init: initRoadmap,
    addBlock: addBlock,
    deleteSelected: deleteSelected,
    saveRoadmap: saveRoadmap,
    loadRoadmap: loadRoadmap,
    loadFromLocalStorage: loadFromLocalStorage,
    loadFromFile: loadFromFile,
    getAvailableShapes: getAvailableShapes,
    getAvailableColors: getAvailableColors,
    clearDiagram: clearDiagram
};