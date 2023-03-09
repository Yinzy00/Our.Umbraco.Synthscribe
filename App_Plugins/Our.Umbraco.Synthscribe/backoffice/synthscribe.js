$http = angular.injector(["ng"]).get("$http");

console.log("Synthscribe Loaded");

var debounceTimerTextboxes;
const debounceTextboxes = (func, timeout = 500) => {
    clearTimeout(debounceTimerTextboxes);
    debounceTimerTextboxes = setTimeout(() => { func(); }, timeout);
}

var debounceTimerTextareas;
const debounceTextAreas = (func, timeout = 500) => {
    clearTimeout(debounceTimerTextareas);
    debounceTimerTextareas = setTimeout(() => { func(); }, timeout);
}

var debounceTimerRte;
const debounceRte = (func, timeout = 500) => {
    clearTimeout(debounceTimerRte);
    debounceTimerRte = setTimeout(() => { func(); }, timeout);
}



//on window load
window.onload = async () => {

    setTimeout(() => {

        setSynthscribeEventHandlers();

    }, 1000);

}

const setSynthscribeEventHandlers = async () => {

    console.log("Setting Event Handlers");

    //#region clickables

    var clickables = [];

    //Block List Create Buttons
    //.umb-block-list__create-button

    //Buttons
    clickables = clickables.concat([...document.querySelectorAll(`button`)]);

    //Links
    clickables = clickables.concat([...document.querySelectorAll(`a`)]);


    clickables.forEach(clickable => {

        if (!isSynthscribeDataAttributeSet(clickable)) {

            clickable.addEventListener("click", () => {

                setTimeout(() => {

                    setSynthscribeEventHandlers();

                }, 1000);

            });
            setSynthscribeDataAttribute(clickable);
        }
    });

    //#endregion clickables

    //#region textfields

    //Textfields
    var textboxes = [...document.querySelectorAll(`input[type="text"]`)];

    //Textareas
    var textareas = [...document.querySelectorAll(`textarea`)];

    console.log((textboxes.length + textareas.length) + " Textfields found");
    textboxes.forEach(field => {

        if (!isSynthscribeDataAttributeSet(field)) {
            field.addEventListener("keydown", async (e) => {

                if (isFetching(e.target))
                    e.preventDefault();

            });

            field.addEventListener("keyup", async (e) => {

                if (!isFetching(e.target))
                    debounceTextboxes(() => handleSynthscribeEvent(new SynthscribeEvent(0, e.target.value, e.target)));

            });

            setSynthscribeDataAttribute(field);
        }
    });

    textareas.forEach(field => {

        if (!isSynthscribeDataAttributeSet(field)) {
            field.addEventListener("keydown", async (e) => {

                if (isFetching(e.target))
                    e.preventDefault();

            });

            field.addEventListener("keyup", async (e) => {

                if (!isFetching(e.target))
                    debounceTextAreas(() => handleSynthscribeEvent(new SynthscribeEvent(1, e.target.value, e.target)));

            });

            setSynthscribeDataAttribute(field);
        }
    });

    //Richtext Editors
    document.querySelectorAll(`iframe`).forEach(el => {

        var rtes = el.contentDocument.querySelectorAll(`[contenteditable="true"]`);

        console.log(rtes.length + " RTEs found");

        rtes.forEach(rte => {

            if (!isSynthscribeDataAttributeSet(rte)) {
                rte.addEventListener("keydown", async (e) => {

                    if (isFetching(e.target))
                        e.preventDefault();

                });

                rte.addEventListener("keyup", async (e) => {

                    if (!isFetching(e.target))
                        debounceRte(() => handleSynthscribeEvent(new SynthscribeEvent(2, e.target.innerText, e.target)));

                });

                setSynthscribeDataAttribute(rte);
            }
        });
    });
    //#endregion textfields
}

const handleSynthscribeEvent = async (e) => {

    if (!isFetching(e.target)) {
        console.log(e);

        var value = null;
        if (e?.data.startsWith("{?") && e?.data.endsWith(";}")) {

            toggleIsFetching(e.target);

            var context = e.data.replace("{?", "").replace(";}", "");

            if (context.toLowerCase() == "rnd") {
                switch (e?.type) {
                    case 0:
                        e.target.value = `Lorem ipsum dolor sit amet, consectetur adipiscing elit.`;
                        break;
                    case 1:
                        e.target.value = `Lorem ipsum dolor sit amet, consectetur adipiscing elit. Aliquam dictum ante eget purus suscipit, sit amet porttitor eros pharetra. Integer aliquet, tellus eget porta rutrum, enim nibh venenatis massa, id imperdiet eros enim eu velit. Donec malesuada libero eu ante lacinia egestas. Aenean a tempor quam, in euismod nibh. Vestibulum ante ipsum primis in faucibus orci luctus et ultrices posuere cubilia curae; Ut id laoreet justo. Aenean egestas lectus vel lacus luctus consectetur. Proin tempus neque sollicitudin nulla feugiat malesuada. Ut velit ex, ultricies id tempus at, fringilla vitae dolor.`;
                        break;
                    case 2:
                        e.target.childNodes[0].innerHTML = `<p>Lorem ipsum dolor sit amet, consectetur adipiscing elit. Aliquam dictum ante eget purus suscipit, sit amet porttitor eros pharetra. Integer aliquet, tellus eget porta rutrum, enim nibh venenatis massa, id imperdiet eros enim eu velit. Donec malesuada libero eu ante lacinia egestas. Aenean a tempor quam, in euismod nibh. Vestibulum ante ipsum primis in faucibus orci luctus et ultrices posuere cubilia curae; Ut id laoreet justo. Aenean egestas lectus vel lacus luctus consectetur. Proin tempus neque sollicitudin nulla feugiat malesuada. Ut velit ex, ultricies id tempus at, fringilla vitae dolor.</p>

                        <p>Fusce nec libero augue. Nullam quis lacus quis magna imperdiet vulputate vel quis eros. Praesent tempus lacus imperdiet sapien congue, et commodo nibh hendrerit. Pellentesque eleifend, ex ut consectetur lacinia, nunc lorem pulvinar tortor, blandit tincidunt nibh tellus sit amet lorem. Maecenas facilisis sapien eu tellus pulvinar posuere. Nulla lacinia enim enim, non mattis erat feugiat nec. Nam ut libero nec massa accumsan semper eu quis tellus. Pellentesque sit amet convallis sapien, tincidunt dapibus nulla. Fusce aliquet tristique velit, in imperdiet eros commodo quis. Vestibulum convallis convallis posuere. Nam sodales sem nec varius gravida. Ut ac odio lacinia, vulputate dolor quis, lacinia massa. Duis vel dui pretium, feugiat arcu at, luctus felis. In hac habitasse platea dictumst. Fusce egestas neque sit amet sapien malesuada efficitur. Pellentesque felis eros, semper sit amet efficitur placerat, efficitur fringilla ante.</p>
                        
                        <p>In ut tortor egestas, viverra magna et, mattis sapien. Vestibulum hendrerit sem quis rutrum laoreet. Curabitur at dui gravida, porttitor est non, interdum libero. Mauris tempus tellus et fermentum pellentesque. Nulla pharetra nibh at lectus ultrices dictum. In lorem diam, cursus non laoreet in, hendrerit eget turpis. Sed venenatis laoreet purus, et posuere risus egestas eget. Orci varius natoque penatibus et magnis dis parturient montes, nascetur ridiculus mus.</p>`;
                }
            }
            else {
                if (e?.type == 2)
                    context += " (return as html content)";

                var body = {
                    "context": context
                };
                var response = await $http({
                    method: "POST",
                    url: "/umbraco/backoffice/Synthscribe/TextContent/GenerateText",
                    data: body,
                    headers: {
                        "Content-Type": "application/json"
                    }
                });

                if (response.status == 200) {

                    value = await response.data;



                    switch (e?.type) {
                        case 0:
                            e.target.value = value;
                            break;
                        case 1:
                            e.target.value = value;
                            break;
                        case 2:
                            e.target.childNodes[0].innerHTML = value;
                    }
                }
                else {
                    console.log("Error: " + response.status);
                }
            }

            toggleIsFetching(e.target);

            //Trigger change for angular to update the model
            var event = new Event('change');

            e.target.dispatchEvent(event);

        }
    }
}

const setSynthscribeDataAttribute = (element) => {
    element.setAttribute("data-synthscribe", "true");
}

const isSynthscribeDataAttributeSet = (element) => {
    return element.getAttribute("data-synthscribe") == "true";
}

const toggleIsFetching = (element) => {
    element.setAttribute("data-synthscribe-fetching", !isFetching(element));
}

const isFetching = (element) => {
    return element.getAttribute("data-synthscribe-fetching") == "true";
}


class SynthscribeEvent {

    type = null;
    data = null;
    target = null;

    constructor(type, data, target) {

        //0 => Textfield
        //1 => Textarea
        //2 => RTE
        this.type = type;
        this.data = data;
        this.target = target

    }
}