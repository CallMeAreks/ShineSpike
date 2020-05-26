$(function () {
    var editor = editormd("editormd", {
        width: "100%",
        height: "70vh",
        path: '/lib/',
        toolbarIcons: function () {
            return [
                "bold", "del", "italic", "quote", "|",
                "h1", "h2", "h3", "h4", "h5", "h6", "|",
                "list-ul", "list-ol", "|",
                "link", "reference-link", "image", "code", "preformatted-text", "code-block", "table", "|",
                "preview", "fullscreen"
            ]
        },

        codeFold: true,
        searchReplace: true,
        saveHTMLToTextarea: true,
        htmlDecode: "style,script,iframe|on*",
        emoji: true,
        taskList: true,
        tocm: true,
        tex: true,
        imageUpload: true,
        //previewCodeHighlight : false,
        flowChart: true,
        sequenceDiagram: true,
        onload: function () {
            this.setTheme("dark");
            this.setEditorTheme("neat");
        },
        onfullscreen: function () {
            $("#post-actions, .navbar").hide();
        },
        onfullscreenExit: function () {
            $("#post-actions, .navbar").show();
        },
    });
});