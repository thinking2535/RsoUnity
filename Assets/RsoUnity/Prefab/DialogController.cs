using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace rso.unity
{
    public class DialogController
    {
        public class Data
        {
            public Dialog dialog;
            public CancellationTokenSource cancellationTokenSource;
            public object returnValue;

            public Data(Dialog dialog, CancellationTokenSource cancellationTokenSource)
            {
                this.dialog = dialog;
                this.cancellationTokenSource = cancellationTokenSource;
            }
            public void cancelToken(object returnValue)
            {
                this.returnValue = returnValue;
                cancellationTokenSource.Cancel();
            }
        }

        string _sortingLayerName;
        float _startPlaneDistance;
        public Stack<Data> datas { get; private set; } = new Stack<Data>();
        public Int32 count => datas.Count;
        public bool isEmpty => datas.Count == 0;
        public bool isNotEmpty => datas.Count > 0;
        float _getTotalThickness()
        {
            float thickness = 0.0f;

            foreach (var i in datas)
                thickness += i.dialog.thickness;

            return thickness;
        }
        float _getTotalPlaneDistance()
        {
            return _startPlaneDistance - _getTotalThickness();
        }

        public DialogController(string sortingLayerName, float startPlaneDistance)
        {
            _sortingLayerName = sortingLayerName;
            _startPlaneDistance = startPlaneDistance;
        }
        ~DialogController()
        {
            clear();
        }
        public async Task<object> push(Dialog dialog, Camera camera)
        {
            dialog.canvas.worldCamera = camera;
            dialog.canvas.sortingLayerName = _sortingLayerName;
            dialog.canvas.sortingOrder = datas.Count;
            dialog.canvas.planeDistance = _getTotalPlaneDistance();

            var data = new Data(dialog, new CancellationTokenSource());

            datas.Push(data);

            await Task.Run(
                () =>
                {
                    while (true)
                    {
                        if (data.cancellationTokenSource.Token.IsCancellationRequested)
                            break;

                        Task.Delay(1).Wait();
                    }
                },
                data.cancellationTokenSource.Token);

            GameObject.Destroy(data.dialog.gameObject);
            return data.returnValue;
        }
        public void pop(object returnValue = null)
        {
            if (isEmpty)
                return;

            datas.Pop().cancelToken(returnValue);
        }
        public void clear()
        {
            while (isNotEmpty)
                pop();
        }
        public async Task<bool> sendBackButtonPressedEvent()
        {
            if (isEmpty)
                return false;

            await datas.Peek().dialog.backButtonPressed();
            return true;
        }
        public Dialog peek()
        {
            if (isEmpty)
                return null;

            return datas.Peek().dialog;
        }
    }
}