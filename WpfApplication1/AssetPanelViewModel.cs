using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApplication1
{
	public class MaterialFile : ViewModelBase
	{
		public string FullPath { get; } = Path.GetRandomFileName();
	}
	class AssetPanelViewModel : ToolViewModel, IDisposable
	{
		CompositeDisposable Disposable { get; } = new CompositeDisposable();

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public AssetPanelViewModel() : base("アセット")
		{
			Models = new ReactiveCollection<ModelFileViewModel>();
			{
				var dummyModel = new ModelFileViewModel("ダミー(全てのマテリアル)");
				Enumerable.Range(0, 10).ToList().ForEach(i => dummyModel.MaterialSlots.Add(new MaterialFileViewModel(i)));
				Models.Add(dummyModel);

				var dummy2 = new ModelFileViewModel("ダミー2");
				Enumerable.Range(11, 10).ToList().ForEach(i => dummy2.MaterialSlots.Add(new MaterialFileViewModel(new MaterialFile())));
				Models.Add(dummy2);
			}

			SelectedModels = new ReactiveCollection<ModelFileViewModel>();

			//SelectedModels = Models.ObserveElementObservableProperty(m => m.IsSelected).Where(_ => _.Value).Select(_ => _.Instance).ToReactiveCollection().AddTo(Disposable);

			//var selectedModels = new ReactiveCollection<ModelFileViewModel>();
			//SelectedModels = selectedModels.ToReadOnlyReactiveCollection().AddTo(Disposable);

			//SelectedModels = new ReactiveCollection<ModelFileViewModel>();
			//Models.ObserveElementObservableProperty(m => m.IsSelected)
			//	.Subscribe(_ => {
			//		if (_.Value)
			//		{
			//			SelectedModels.Add(_.Instance);
			//		}
			//		else
			//		{
			//			SelectedModels.Remove(_.Instance);
			//		}
			//	}).AddTo(Disposable);

			IsMultiSelected = SelectedModels.CollectionChangedAsObservable().Select(_ => SelectedModels.Count > 1).ToReadOnlyReactiveProperty(SelectedModels.Count > 0).AddTo(Disposable);

			Models.ObserveElementObservableProperty(m => m.IsSelected).Subscribe(_ =>
			{
				Console.WriteLine($"{_.Instance.Name.Value} : {_.Value}, {_.Instance.IsSelected.Value}");
			}).AddTo(Disposable);
		}

		/// <summary>
		/// モデル一覧の取得
		/// </summary>
		public ReactiveCollection<ModelFileViewModel> Models { get; }

		/// <summary>
		/// 選択したモデル一覧の取得
		/// </summary>
		public ReactiveCollection<ModelFileViewModel> SelectedModels { get; }

		public ReadOnlyReactiveProperty<bool> IsMultiSelected { get; }

		/// <summary>
		/// マテリアルVM
		/// </summary>
		public class MaterialFileViewModel : IDisposable
		{
			CompositeDisposable Disposable { get; } = new CompositeDisposable();

			/// <summary>
			/// コンストラクタ
			/// </summary>
			/// <remarks>
			/// 空スロット
			/// </remarks>
			internal MaterialFileViewModel(int i)
			{
				Name = new ReactiveProperty<string>("マテリアルスロット" + i).ToReadOnlyReactiveProperty().AddTo(Disposable);
			}

			/// <summary>
			/// コンストラクタ
			/// </summary>
			/// <param name="material">マテリアルファイル</param>
			internal MaterialFileViewModel(MaterialFile material)
			{
				m_Material = material;
				Name = material.ObserveProperty(m => m.FullPath).Select(p => Path.GetFileName(p)).ToReadOnlyReactiveProperty().AddTo(Disposable);
			}

			private readonly MaterialFile m_Material = null;
			/// <summary>
			/// 空スロットか取得
			/// </summary>
			public bool IsEmpty => null == m_Material;

			/// <summary>
			/// 名前プロパティの取得
			/// </summary>
			public ReadOnlyReactiveProperty<string> Name { get; }

			/// <summary>
			/// 選択されたかプロパティの取得
			/// </summary>
			public ReactiveProperty<bool> IsSelected { get; } = new ReactiveProperty<bool>(false);

			/// <summary>
			/// 
			/// </summary>
			public void Dispose()
			{
				Disposable.Dispose();
			}
		}


		/// <summary>
		/// モデルVM
		/// </summary>
		public class ModelFileViewModel : IDisposable
		{
			CompositeDisposable Disposable { get; } = new CompositeDisposable();

			/// <summary>
			/// コンストラクタ
			/// </summary>
			internal ModelFileViewModel(string name)
			{
				Name = new ReactiveProperty<string>(name).ToReadOnlyReactiveProperty().AddTo(Disposable);
				MaterialSlots = new ReactiveCollection<MaterialFileViewModel>();
			}

			/// <summary>
			/// 名前プロパティの取得
			/// </summary>
			public ReadOnlyReactiveProperty<string> Name { get; }

			/// <summary>
			/// マテリアル一覧の取得
			/// </summary>
			public ReactiveCollection<MaterialFileViewModel> MaterialSlots { get; }

			/// <summary>
			/// 選択されたかプロパティの取得
			/// </summary>
			public ReactiveProperty<bool> IsSelected { get; } = new ReactiveProperty<bool>(false);

			/// <summary>
			/// 
			/// </summary>
			public void Dispose()
			{
				Disposable.Dispose();
			}
		}


		/// <summary>
		/// 
		/// </summary>
		public void Dispose()
		{
			Disposable.Dispose();
		}

	}
}
