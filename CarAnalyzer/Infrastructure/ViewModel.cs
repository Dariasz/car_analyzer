using System;
using System.Reactive.Disposables;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace CarAnalyzer.Infrastructure
{
    public abstract class ViewModel : ReactiveObject
    {
        CompositeDisposable deactivateWith;
        protected CompositeDisposable DeactivateWith
        {
            get
            {
                if (this.deactivateWith == null)
                    this.deactivateWith = new CompositeDisposable();

                return this.deactivateWith;
            }
        }

        protected CompositeDisposable DestroyWith { get; } = new CompositeDisposable();


        public virtual void OnDisappearing()
        {
            this.deactivateWith?.Dispose();
            this.deactivateWith = null;
        }


        public virtual void Destroy()
        {
            this.DestroyWith?.Dispose();
        }


        [Reactive] public bool IsBusy { get; protected set; }
    }
}