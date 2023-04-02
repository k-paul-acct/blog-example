function renderPost(s) {
    const editor = new EditorJS({
        holder: 'post-body',
        autofocus: false,
        data: s
    });
}