function invokeClickFor(elementId) {
    document.getElementById(elementId).click();
}

function RegisterScrollTracking(dotnetHelper, callbackName) {
    let flag = false;
    window.addEventListener('scroll', () =>
    {
        let element = document.scrollingElement;

        if (element.scrollHeight - element.scrollTop - 100 <= element.clientHeight)
        {
            if(!flag) {
                dotnetHelper.invokeMethodAsync(callbackName);
                flag = true;
            }
        }
        else {
            flag = false;
        }
    });
}