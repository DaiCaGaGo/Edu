/**
 * @license Copyright (c) 2003-2019, CKSource - Frederico Knabben. All rights reserved.
 * For licensing, see https://ckeditor.com/legal/ckeditor-oss-license
*/
CKEDITOR.editorConfig = function (config) {
    // Define changes to default configuration here. For example:
    // config.language = 'fr';
    // config.uiColor = '#AADC6E';

    config.syntaxhighlight_lang = 'csharp';
    config.syntaxhighlight_hideControls = true;
    config.language = 'vi';
    config.filebrowserBrowseUrl = baseUrlCustomer + 'Assets/ckfinder/ckfinder.html';
    config.filebrowserImageBrowseUrl = baseUrlCustomer + 'Assets/ckfinder/ckfinder.html?Type=Images';
    config.filebrowserFlashBrowseUrl = baseUrlCustomer + 'Assets/ckfinder/ckfinder.html?Type=Flash';
    config.filebrowserUploadUrl = baseUrlCustomer + 'Assets/ckfinder/core/connector/aspx/connector.aspx?command=QuickUpload&type=Files';
    config.filebrowserImageUploadUrl = baseUrlCustomer + 'push/nobody/';
    config.filebrowserFlashUploadUrl = baseUrlCustomer + 'Assets/ckfinder/core/connector/aspx/connector.aspx?command=QuickUpload&type=Flash';

    CKFinder.setupCKEditor(null, baseUrlCustomer + 'Assets/ckfinder/');
    CKEDITOR.config.entities = false;
    config.basicEntities = false;   // xoa ki tu &nbsp trong noi dung
    config.allowedContent = true;
};
