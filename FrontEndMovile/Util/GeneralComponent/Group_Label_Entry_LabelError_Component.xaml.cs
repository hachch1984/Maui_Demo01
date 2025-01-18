namespace FrontEndMovile.Util.GeneralComponent
{
    public partial class Group_Label_Entry_LabelError_Component : ContentView
    {
        public Group_Label_Entry_LabelError_Component()
        {
            InitializeComponent();
        }

        #region Label 
        // LabelText (Texto del Label superior)
        public static readonly BindableProperty LabelTextProperty =
            BindableProperty.Create(nameof(LabelText), typeof(string), typeof(Group_Label_Entry_LabelError_Component), string.Empty);

        public string LabelText
        {
            get => (string)GetValue(LabelTextProperty);
            set => SetValue(LabelTextProperty, value);
        }

        #endregion




        #region Entry
        /// <summary>
        /// Limpia el texto del Entry
        /// </summary>
        public void EntryClearText()
        {
            EntryBinding = string.Empty;
        }


        // EntryBinding (Texto capturado por el Entry)
        public static readonly BindableProperty EntryBindingProperty =
            BindableProperty.Create(nameof(EntryBinding), typeof(string), typeof(Group_Label_Entry_LabelError_Component), string.Empty, BindingMode.TwoWay);
        public string EntryBinding
        {
            get => (string)GetValue(EntryBindingProperty);
            set => SetValue(EntryBindingProperty, value);
        }


        // EntryPlaceholder (Placeholder del Entry)
        public static readonly BindableProperty EntryPlaceholderProperty =
            BindableProperty.Create(nameof(EntryPlaceholder), typeof(string), typeof(Group_Label_Entry_LabelError_Component), string.Empty);
        public string EntryPlaceholder
        {
            get => (string)GetValue(EntryPlaceholderProperty);
            set => SetValue(EntryPlaceholderProperty, value);
        }


        //EntryIsPassword (Si el Entry es de tipo Password)
        public static readonly BindableProperty EntryIsPasswordProperty =
            BindableProperty.Create(nameof(EntryIsPassword), typeof(bool), typeof(Group_Label_Entry_LabelError_Component), false);
        public bool EntryIsPassword
        {
            get => (bool)GetValue(EntryIsPasswordProperty);
            set => SetValue(EntryIsPasswordProperty, value);
        }

        //EntryKeyboard (Tipo de teclado del Entry)
        public static readonly BindableProperty EntryKeyboardProperty =
            BindableProperty.Create(nameof(EntryKeyboard), typeof(Keyboard), typeof(Group_Label_Entry_LabelError_Component), Keyboard.Default);
        public Keyboard EntryKeyboard
        {
            get => (Keyboard)GetValue(EntryKeyboardProperty);
            set => SetValue(EntryKeyboardProperty, value);
        }



        #endregion




        #region Label_Error_Component
        // ErrorBinding (Texto del error para Label_Error_Component)
        public static readonly BindableProperty ErrorBindingProperty =
            BindableProperty.Create(nameof(ErrorBinding), typeof(string), typeof(Group_Label_Entry_LabelError_Component), string.Empty);

        public string ErrorBinding
        {
            get => (string)GetValue(ErrorBindingProperty);
            set => SetValue(ErrorBindingProperty, value);
        }
        
        #endregion

    }
}
